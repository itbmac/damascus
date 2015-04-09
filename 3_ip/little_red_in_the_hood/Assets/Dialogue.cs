using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

struct dialogueLine {
	public string character;
	public string dialogue;

	public dialogueLine(string c, string d){
		character = c;
		dialogue = d;
	}
}

public class Dialogue : MonoBehaviour {
	//Current line index being read.
	int index = 0;
	//The last line index that was visually rendered.
	int lastRendered = -1;

	//I'll make this a list later....
	dialogueLine[] dialogueLines;

	//This will be reinitialized in Start() once I start reading in files.
	int numLines = 9;

	//Will initialize these privately later
	public GameObject dialogue_top;
	public GameObject dialogue_bottom;
	public GameObject dialogue_bottom_off;
	public GameObject dialogue_top_off;

	Text text1, text2, text3, text4;

	//The path of the file containing the dialogue.
	public string file_path;

	// Use this for initialization
	void Start () {
		//Initialize variables.
		text1 = dialogue_bottom_off.GetComponent<Text>();
		text2 = dialogue_bottom.GetComponent<Text>();
		text3 = dialogue_top.GetComponent<Text>();
		text4 = dialogue_top_off.GetComponent<Text>();

		//Set background.

		//Read in dialogue file.
		StreamReader inp_stm = new StreamReader(file_path);

		//Read the first line, parse, and use this information.
		string inp_ln = inp_stm.ReadLine( );
		string[] words;
		char[] delimiter = {':'};

		dialogueLines = new dialogueLine[numLines];

		int i = 0;
		while(!inp_stm.EndOfStream)
		{
			inp_ln = inp_stm.ReadLine( );
			// Do Something with the input. 
			print (inp_ln);
			words = inp_ln.Split(delimiter);
			dialogueLines[i] = new dialogueLine(words[0], words[1]);
			print (dialogueLines[i].character + ": " + dialogueLines[i].dialogue);
			i++;
		}
		inp_stm.Close( ); 

		//Load speaking characters.

		//Load speech bubbles.

	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown ("space") || Input.GetMouseButtonDown(0)) && (lastRendered == index)){
			index++;
		}
		
		//If the user advances the dialogue but there's none left, go to the next act.
		//for now, this doesn't do anything.
		if(index == dialogueLines.Length){
			//advance to next scene
		}
		//Check if the user has advanced the dialogue.
		else if(lastRendered != index){
			//Render the next line of dialogue offscreen.
			text1.text = dialogueLines[index].dialogue;

			//Move up the top onscreen dialogue off the screen.
			text4.text = text3.text;

			//Move up the bottom onscreen dialogue to the top onscreen dialogue slot.
			text3.text = text2.text;

			//Move up the newly rendered dialogue to the bottom onscreen dialogue slot.
			text2.text = text1.text;
			lastRendered++;
		}
	}
}
