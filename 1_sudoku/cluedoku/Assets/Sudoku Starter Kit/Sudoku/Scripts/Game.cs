using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public GameObject[] box1;			// 9 fields per box
	public GameObject[] box2;			//
	public GameObject[] box3;			//
	public GameObject[] box4;			//
	public GameObject[] box5;			//
	public GameObject[] box6;			//
	public GameObject[] box7;			//
	public GameObject[] box8;			//
	public GameObject[] box9;			//
	
	public Texture[] num;						// number textures
	public Texture[] lockNum;			// locked number textures
	public GUITexture[] buttons;		// gui texture buttons
	
	public GameObject selected = null;		// selected field
	
	Field f;																// field script reference
	
	public GameObject numButtons;			// number buttons
	public GameObject gameButtons;		// game menu buttons
	
	float gameTime = 0.0f;								// game play time
	int hrs = 0;														// hours
	int min = 0;														// minutes
	int sec = 0;														// seconds
	string timeFormat = "";								// play time as string for gui text
	bool countTime = false;								// count game time
	public bool solved = false;							// is puzzle solved
	
	int[,] code = new int[10,10];						// solved puzzle
	
	public GameObject texSolved;				// solved gui texture
	public GameObject texFailed;					// failed gui texture
	
	public GameObject gen;							// generating puzzle gui texture
	public AudioClip clickSound;					// click sound
	
	void Start (){
		gen.SetActive(false);								// disable some objects
		texSolved.SetActive(false);
		texFailed.SetActive(false);
		LoadPuzzle();
		RemoveNumbers();									// remove some numbers
		countTime = true;										// start counting time
	}
	
	void Update(){
		if(selected != null){									// object selecteed
			selected.renderer.material.mainTexture = num[10];			// set texture
			string n = selected.name;															// get object name
			numButtons.SetActive(true);													// enable number buttons
			gameButtons.SetActive(false);												// disable menu buttons
			SetGUIButtons(n);																		// select available numbers
		}
		if(countTime){
			CountTime();																					// count time
		}
		string svol = PlayerPrefs.GetString("soundvolume","on");		// get sound volume
		
		if(svol == "on"){
			audio.volume = 1.0f;																				// set sound volume
		}else{
			audio.volume = 0.0f;
		}
	}
	
	void LateUpdate(){
		if(!solved){
			CheckSolve();											// check is puzzle solved
		}
	}
	
	// count game time
	void CountTime(){
		gameTime += 1*Time.deltaTime;		// count time
		timeFormat = "";										// clear time string
		
		sec = (int)(gameTime % 60.0f);			// get seconds
		min = (int)(gameTime / 60.0f);				// get minutes
		hrs = (int)(gameTime / 3600.0f);			// get hours
		
		if(hrs < 10){
			timeFormat = timeFormat + "0"+hrs.ToString()+":";		// create hours string
		}else{
			timeFormat = timeFormat +hrs.ToString()+":";
		}
		if(min < 10){
			timeFormat = timeFormat + "0"+min.ToString()+":";		// create minutes string
		}else{
			timeFormat = timeFormat +min.ToString()+":";
		}
		if(sec < 10){
			timeFormat = timeFormat + "0"+sec.ToString();				// create seconds string
		}else{
			timeFormat = timeFormat +sec.ToString();
		}
		GameObject.Find("GameTime").guiText.text = timeFormat;		// set gui text
	}
	
	// check if puzzle is solved
	void CheckSolve(){
		int filled = 0;										// number of filled fields
		Field[] f = FindObjectsOfType(typeof(Field)) as Field[];			// find all fields
		foreach(Field fl in f) {	
			if(fl.value != 0){			// check if not empty
				filled++;						// add filled
			}
		}
		
		if(filled == 81){										// if all filled
			countTime = false;							// stop counting
			solved = true;										// puzzle is solved
			texSolved.SetActive(true);				// show gui texture
			SaveScore();										// save current score
			SwitchMenu();									// change menu
		}
	}
	
	// save game score if best
	void SaveScore(){
		string gameLevel = "";							// play level
		bool canSave = false;							// can score be saved
		string lb = "";											// last best score
		string cs = GameObject.Find("GameTime").guiText.text;				// get current score
		
		gameLevel = PlayerPrefs.GetString("gamelevel","easy");					// get play level
		
		switch(gameLevel){
			case "easy":																										// if easy
				lb = PlayerPrefs.GetString("easyscore","99:59:59");					// get score
				canSave = CompareScore(cs,lb);															// compare scores
				if(canSave){																									// if can save
					PlayerPrefs.SetString("easyscore",cs);											// save current score
				}
				break;
			case "medium":																								// if medium
				lb = PlayerPrefs.GetString("mediumscore","99:59:59");
				canSave = CompareScore(cs,lb);
				if(canSave){
					PlayerPrefs.SetString("mediumscore",cs);
				}
				break;
			case "hard":																										// if hard
				lb = PlayerPrefs.GetString("hardscore","99:59:59");
				canSave = CompareScore(cs,lb);
				if(canSave){
					PlayerPrefs.SetString("hardscore",cs);
				}
				break;
		}
	}
	
	// compare final score
	bool CompareScore(string ns, string os){									// new score, old score
		bool cs = false;																					// can save
		
		string nhrss = ns[0].ToString()+ns[1].ToString();					// create new hours 
		string nmins = ns[3].ToString()+ns[4].ToString();					// create new minutes
		string nsecs = ns[6].ToString()+ns[7].ToString();					// create new seconds
		
		int nhrsi = int.Parse(nhrss);															// convert all to int
		int nmini = int.Parse(nmins);
		int nseci = int.Parse(nsecs);
		
		string ohrss = os[0].ToString()+os[1].ToString();					// create old hours, minutes and seconds and convert them to int
		string omins = os[3].ToString()+os[4].ToString();
		string osecs = os[6].ToString()+os[7].ToString();
		
		int ohrsi = int.Parse(ohrss);
		int omini = int.Parse(omins);
		int oseci = int.Parse(osecs);
		
		if(nhrsi < ohrsi){						// compare hours, if new better then can save
			cs = true;								
		}
		if(nmini < omini){					// compare minutes
			cs = true;
		}
		if(nseci < oseci){						// compare seconds
			cs = true;
		}
		
		return cs;									// return can save
	}
	
	// clear puzzle
	public void DoClear(){	
		if(!solved){								// not solved
			Field[] f = FindObjectsOfType(typeof(Field)) as Field[];			// find all fields
			foreach(Field fl in f) {
				if(fl.canPlace == true){								// if can place
					fl.value = 0;												// set field value
					fl.gameObject.renderer.material.mainTexture = num[0];		// set field texture
				}
			}
		}
	}
	
	// solve puzzle
	public void DoSolve(){
		if(!solved){												//
			bool cp = false;									// set can place
			string fn = "";										// field name
			for(int x = 1; x < 10; x++){				//
				for(int y = 1; y < 10; y++){			//
					fn = x.ToString() + y.ToString();		// create field name
					cp = GameObject.Find(fn).GetComponent<Field>().canPlace;								// get field can place
					if(cp){																																				// if can place
						GameObject.Find(fn).GetComponent<Field>().value = code[x,y];						// set field value
						GameObject.Find(fn).renderer.material.mainTexture = num[code[x,y]];			// set field texture
					}
				}
			}
			solved = true;								// set solved
			countTime = false;					// stop counting time
			texFailed.SetActive(true);		// show gui texture
		}
	}
	
	// load prefab puzzle
	void LoadPuzzle(){
		string fn = "";						// field name
		int i = -1;								// index number
		int no = 0;							// field number
		
		string puzzle = "";																				// puzzle
		
		puzzle = GameObject.FindWithTag("database").GetComponent<PuzzleDatabase>().SelectPuzzle();				// get puzzle

		for(int x = 1; x < 10; x++){
			for(int y = 1; y < 10; y++){
				fn = x.ToString() + y.ToString();			// create field name
				i++;																	//
				no = int.Parse(puzzle[i].ToString());	// create number
				GameObject.Find(fn).GetComponent<Field>().value = no;								// set value
				GameObject.Find(fn).renderer.material.mainTexture = lockNum[no];			// set texture
				code[x,y] = no;											// save field value
			}
		}
	}
	
	// check column and row
	bool CheckCR(int a, int b, int val){				// first number, second number, value
		bool cp = true;												// can place
		string s = "";													// field name
		for(int x = 1; x < 10; x++){							// loop column
			s = "";																// reset name
			s = s + b.ToString() + x.ToString();		// create name
			f = GameObject.Find(s).GetComponent<Field>();	// find field
			
			if(val == f.value){										// check value
				cp = false;													// set can place
			}
		}
		
		for(int x = 1; x < 10; x++){							// loop row
			s = "";																// reset name
			s = s + x.ToString() + a.ToString();		// create name
			f = GameObject.Find(s).GetComponent<Field>();		// 
			if(val == f.value){																		//
				cp = false;																					//
			}
		}
		return cp;															// return can place
	}
	
	// reset table
	void Reset(){
		Field[] f = FindObjectsOfType(typeof(Field)) as Field[];		// find all fields
		foreach(Field fl in f){
			fl.value = 0;						// reset value
			fl.canPlace = true;			// reset can place
			fl.gameObject.renderer.material.mainTexture = num[0];		// set texture
		}
	}
	
	// check for same numbers in box
	bool CheckBox(int a, int b, int val){				//
		string s = "";														//	field name
		s = s + b.ToString() + a.ToString();				//	create name
		bool p = true;														//	can place
		
		// box 1
		if(s == "11" || s == "12" || s == "13" || s == "21" || s == "22" || s == "23" || s == "31" || s == "32" || s == "33"){		// select name
			p = LoopBox(val, box1);																																						// loop thru box
		}
		// box 2
		if(s == "41" || s == "42" || s == "43" || s == "51" || s == "52" || s == "53" || s == "61" || s == "62" || s == "63"){
			p = LoopBox(val, box2);
		}
		// box 3
		if(s == "71" || s == "72" || s == "73" || s == "81" || s == "82" || s == "83" || s == "91" || s == "92" || s == "93"){
			p = LoopBox(val, box3);
		}
		// box 4
		if(s == "14" || s == "15" || s == "16" || s == "24" || s == "25" || s == "26" || s == "34" || s == "35" || s == "36"){
			p = LoopBox(val, box4);
		}
		// box 5
		if(s == "44" || s == "45" || s == "46" || s == "54" || s == "55" || s == "56" || s == "64" || s == "65" || s == "66"){
			p = LoopBox(val, box5);
		}
		// box 6
		if(s == "74" || s == "75" || s == "76" || s == "84" || s == "85" || s == "86" || s == "94" || s == "95" || s == "96"){
			p = LoopBox(val, box6);
		}
		// box 7
		if(s == "17" || s == "18" || s == "19" || s == "27" || s == "28" || s == "29" || s == "37" || s == "38" || s == "39"){
			p = LoopBox(val, box7);
		}
		// box 8
		if(s == "47" || s == "48" || s == "49" || s == "57" || s == "58" || s == "59" || s == "67" || s == "68" || s == "69"){
			p =LoopBox(val, box8);
		}
		// box 9
		if(s == "77" || s == "78" || s == "79" || s == "87" || s == "88" || s == "89" || s == "97" || s == "98" || s == "99"){
			p = LoopBox(val, box9);
		}
		
		return p;			// return can place
	}
	
	// loop thru box
	bool LoopBox(int a, GameObject[] bx){					// value, box with fields
		bool p = true;																	// can place
		foreach(GameObject b in bx){								// loop box
			int v = b.GetComponent<Field>().value;			// get field value
			if(v == a){																		// if same
				p = false;																	// set can place
			}
		}
		
		return p;																			// return can place
	}
	
	// prepare game, remove some numbers
	void RemoveNumbers(){
		SetBox(box1);																	// remove numbers from box
		SetBox(box2);
		SetBox(box3);
		SetBox(box4);
		SetBox(box5);
		SetBox(box6);
		SetBox(box7);
		SetBox(box8);
		SetBox(box9);
	}
	
	// remove numbers in box
	void SetBox(GameObject[] bx){								// box with fields
		string gameLevel = "";												// game level
		gameLevel = PlayerPrefs.GetString("gamelevel","easy");		// get game level
		
		int noRemoved = 0;													// number of removed fields
		int index = 0;																// field number
		int toRemove = 0;														// number of fields to be removed
		
		switch(gameLevel){
			case "easy":
				toRemove = Random.Range(4,6);				// get number of fields to be removed
				break;
			case "medium":
				toRemove = Random.Range(5,8);
				break;
			case "hard":
				toRemove = Random.Range(6,9);
				break;
		}
		
		do{
			index = Random.Range(0,9);													// random field number
			int val = bx[index].GetComponent<Field>().value;			// get field value
			if(val != 0){																						// if not empty
				bx[index].GetComponent<Field>().value = 0;					// set field value
				bx[index].GetComponent<Field>().canPlace = true;		// set can place
				bx[index].renderer.material.mainTexture = num[0];		// set texture
				noRemoved++;																			// count removed
			}
		}while(noRemoved < toRemove);												// remove until removed = to remove
	}
	
	// set buttons
	void SetGUIButtons(string a){															// a = field name
		int c = int.Parse(a[0].ToString());												// get column number
		int r = int.Parse(a[1].ToString());													// get row number
		bool en = true;																					// enabled
		
		for(int b = 1; b < 10; b++){
			buttons[b].GetComponent<GUIButton>().enable = true;			// set button enabled
			string s = "";																					// name
			for(int x = 1; x < 10; x++){
				s = "";																								// reset name
				s = s + c.ToString() + x.ToString();										// create name
				f = GameObject.Find(s).GetComponent<Field>();		// find field
				if(b == f.value){																			// compare value
					buttons[b].GetComponent<GUIButton>().enable = false;		// set button disabled
				}
			}
			
			for(int x = 1; x < 10; x++){
				s = "";							// reset name
				s = s + x.ToString() + r.ToString();			// create name
				f = GameObject.Find(s).GetComponent<Field>();		// 
				if(b == f.value){																			//
					buttons[b].GetComponent<GUIButton>().enable = false;		//
				}
			}
			
			en = CheckBox(r,c,b);					// check box for number
			if(!en){												// 
				buttons[b].GetComponent<GUIButton>().enable = false;		//
			}
		}
	}
	
	// change menu
	public void SwitchMenu(){
		if(selected != null){												
			selected.renderer.material.mainTexture = num[0];			// set texture
			selected.GetComponent<Field>().value = 0;						// set field value
			selected = null;																				// set selected
		}
		numButtons.SetActive(false);														// hide numbers
		gameButtons.SetActive(true);														// show game menu
	}
	
	public void PlayClick(){
		audio.PlayOneShot(clickSound);													// play sound
	}
}


