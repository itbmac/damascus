using UnityEngine;
using System.Collections;

public class animateFinal : MonoBehaviour {
	bool loading = false;
	SpriteRenderer render;
	int j = 0;
	string[] researcherDialogue = new string[]{"it appears the \nsubject has finished.",
		"emotions have also \nbeen well-simulated.",
		"our experiment was \na success then.",
		"finally, we \nhave created true \nartificial intelligence."};
	string[] robotDialogue = new string[]{"......WHAT?",
		"I WAS...AN EXPERIMENT ALL ALONG?",
		"I JUST WANTED TO REGAIN MY SENSES BUT...",
		"WHAT DO I DO NOW?",
		"..............",
		"IF MY ACTIONS AREN'T TRULY MY OWN...",
		"then i'd rather not see at all."};
	GUIText researcherTextbox;
	GUIText robotTextbox;
	int turn = 0; //0 is researcher's turn, 1 is robot's turn, 2 is fadeout

	int frameCount = 5;
	int currentFrame = 0;
	GUITexture blackscreen;
	bool done = false;
	
	IEnumerator fadeToBlack(){
		float timeLeft = .5f;
		Color col = blackscreen.color;
		col.a = 0;
		
		for(float f = 0f; f <= 1f; f += .001f){
			col.a = f;
			blackscreen.color = col;
			yield return null;
			
			while ((timeLeft -= Time.deltaTime) > 0.0f)
			{
				yield return null;
			}
		}
		
		done = true;
	}

	IEnumerator animate(){
		while(true){
			float timeLeft = .5f;
			//change frame
			gameObject.transform.GetChild(currentFrame).renderer.enabled = false;
			currentFrame = (currentFrame + 1) % frameCount;
			gameObject.transform.GetChild(currentFrame).renderer.enabled = true;
			while ((timeLeft -= Time.deltaTime) > 0.0f)
			{
				yield return null;
			}
		}
	}

	IEnumerator nextDialogue(){
		loading = true;

		if(turn == 0){
			researcherTextbox.text = researcherDialogue[j];
			robotTextbox.text = "";
		}
		else if(turn == 1){
			robotTextbox.text = robotDialogue[j];
			researcherTextbox.text = "";
		}
		j++;

		//fade in new text
		for(float i = 0f; i <= 1f; i +=.02f){
			yield return null;
		}

		loading = false;
	}

	// Use this for initialization
	void Start () {
		blackscreen = GameObject.Find("blackscreen").GetComponent<GUITexture>();
		render = gameObject.GetComponent<SpriteRenderer>();
		StartCoroutine(animate());
		researcherTextbox = GameObject.Find("researcherText").GetComponent<GUIText>() as GUIText;
		researcherTextbox.pixelOffset = new Vector2(-300, 100);
		robotTextbox = GameObject.Find("robotText").GetComponent<GUIText>();
		robotTextbox.pixelOffset = new Vector2(0, 20);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			//if(j >= dialogue.Length) Application.LoadLevel(1); 
			if(!loading){
				if(turn == 0){print ("0");
					if(j >= researcherDialogue.Length){
						turn = 1;
						j = 0;
					}
					else StartCoroutine(nextDialogue());
				}
				else if(turn == 1){print ("1");
					if(j >= robotDialogue.Length){
						turn = 2;
						j = 0;
					}
					else StartCoroutine(nextDialogue());
				}
				else{ //turn == 2
					loading = true;
					StartCoroutine(fadeToBlack());
					CrashPlayer.reference.playCrash();
				}
			}
		}


		if (done) {
			StopAllCoroutines();
			Application.LoadLevel(0); 
		}
	}
}
