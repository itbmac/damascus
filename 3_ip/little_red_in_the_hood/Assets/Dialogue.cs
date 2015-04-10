/*
 * DIALOGUE SYSTEM
 * 
 * Notes:
 * 	-currently only supports 2 speakers
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Linq;

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
	int lastRendered = 0;

	//I'll make this a list later....
	dialogueLine[] dialogueLines;

	//This will be reinitialized in Start() once I start reading in files.
	int numLines = 9;

	//Will initialize these privately later
	public GameObject dialogue_top;
	public GameObject dialogue_bottom;
	public GameObject dialogue_bottom_off;
	public GameObject dialogue_top_off;
	public GameObject top_speech_bubble;
	public GameObject bottom_speech_bubble;
	public GameObject left_sprite;
	public GameObject right_sprite;
	public TextAsset MyText;

	//Sprites
	//sprite1 corresponds with speaker1, sprite2 corresponds with speaker2.
	//speaker1 on the left, speaker2 on the right.
	public Sprite sprite1, sprite2;
	string speaker1, speaker2;

	Text text1, text2, text3, text4;
	SpriteRenderer bubble1, bubble2;

	//The next scene after this dialogue.
	public string nextScene;
	
	string[] Lines;

	// Use this for initialization
	void Start () {
		//Initialize variables.
		text1 = dialogue_bottom_off.GetComponent<Text>();
		text2 = dialogue_bottom.GetComponent<Text>();
		text3 = dialogue_top.GetComponent<Text>();
		text4 = dialogue_top_off.GetComponent<Text>();
		bubble1 = top_speech_bubble.GetComponent<SpriteRenderer>();
		bubble2 = bottom_speech_bubble.GetComponent<SpriteRenderer>();
		left_sprite.GetComponent<SpriteRenderer> ().sprite = sprite1;
		right_sprite.GetComponent<SpriteRenderer> ().sprite = sprite2;

		//Set background.

		//Read in dialogue file.
		Lines = MyText.text.Split (new char[] {'\r', '\n'}, System.StringSplitOptions.RemoveEmptyEntries);

		//Read the first line, parse, and use this information.
		string inp_ln_first = Lines[0];

		// The first line indicates the two speakers, and how many lines of dialogue.
		char[] delin = {' '};
		string[] temp = inp_ln_first.Split(delin);
		speaker1 = temp[0];
		speaker2 = temp[1];
		numLines = int.Parse(temp[2]);

		string[] words;
		char[] delimiter = {':'};

		dialogueLines = new dialogueLine[numLines];

		int i = 0;
		foreach (var inp_ln in Lines.Skip(1))
		{
			//This is for testing do not remove yet 8^)
			print (inp_ln);

			words = inp_ln.Split(delimiter);
			dialogueLines[i] = new dialogueLine(words[0], words[1]);
			print (dialogueLines[i].character + ": " + dialogueLines[i].dialogue);
			i++;
		}

		//Load speaking characters.

		//Load speech bubbles.

		//Load the first line of text.
		text2.text = dialogueLines[0].dialogue;
		print (dialogueLines[0].character + ", " + speaker2);
		if(dialogueLines[0].character == speaker2){
			bottom_speech_bubble.transform.eulerAngles = new Vector3(0, 180, 0);
		}

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
			if(nextScene != ""){
				Application.LoadLevel(nextScene);
			}
		}
		//Check if the user has advanced the dialogue.
		else if(lastRendered != index){
			//Render the next line of dialogue offscreen.
			text1.text = dialogueLines[index].dialogue;

			//Move up the top onscreen dialogue off the screen.
			text4.text = text3.text;

			//Move up the bottom onscreen dialogue to the top onscreen dialogue slot.
			text3.text = text2.text;
			bubble1.sprite = bubble2.sprite;
			Vector3 temp = top_speech_bubble.transform.eulerAngles;
			temp.y = bottom_speech_bubble.transform.eulerAngles.y;
			top_speech_bubble.transform.eulerAngles = temp;

			//Move up the newly rendered dialogue to the bottom onscreen dialogue slot.
			text2.text = text1.text;
			//sprite2.sprite;
			temp = bottom_speech_bubble.transform.eulerAngles;

			//This needs to be worked out better but whatever
			if(dialogueLines[index].character != "Red"){
				temp.y = 180;
				bottom_speech_bubble.transform.eulerAngles = temp;
			}
			else{
				temp.y = 0;
				bottom_speech_bubble.transform.eulerAngles = temp;
			}

			lastRendered++;
		}
	}
}
