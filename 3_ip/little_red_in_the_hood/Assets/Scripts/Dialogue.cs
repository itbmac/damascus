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

	GameObject dialogue_top;
	GameObject dialogue_bottom;
	GameObject dialogue_bottom_off;
	GameObject dialogue_top_off;
	GameObject top_speech_bubble;
	GameObject top_off_speech_bubble;
	GameObject bottom_speech_bubble;
	GameObject bottom_off_speech_bubble;
	GameObject left_sprite;
	GameObject right_sprite;

	//The text file we read from.
	public TextAsset MyText;

	//Sprites
	//sprite1 corresponds with speaker1, sprite2 corresponds with speaker2.
	//speaker1 on the left, speaker2 on the right.
	public Sprite Sprite1, Sprite2;
	
	string speaker1, speaker2;

	Text text1, text2, text3;
	SpriteRenderer bubble1, bubble2, bubble3;

	//The next scene after this dialogue.
	public string NextScene;
	
	string[] Lines;

	//Basically a mutex so the player can't skip through text.
	bool transitioning = false;

	//Transitioning speech bubbles.
	IEnumerator MoveSpeechBubbles() {
		transitioning = true;
		float distance1, distance2, distance3;
		distance1 = Vector3.Distance(top_off_speech_bubble.transform.position, top_speech_bubble.transform.position);
		distance2 = Vector3.Distance(top_speech_bubble.transform.position, bottom_speech_bubble.transform.position);
		distance3 = Vector3.Distance(bottom_speech_bubble.transform.position, bottom_off_speech_bubble.transform.position);

		Vector3 temp;

		for (float f = 1f; f >= 0; f -= 0.05f) {
			//top to top-off
			temp = top_speech_bubble.transform.position;
			temp.y += distance1 * 0.1f;
			top_speech_bubble.transform.position = temp;

			//bottom to top
			temp = bottom_speech_bubble.transform.position;
			temp.y += distance2 * 0.1f;
			bottom_speech_bubble.transform.position = temp;

			//bottom-off to bottom
			temp = bottom_off_speech_bubble.transform.position;
			temp.y += distance3 * 0.1f;
			bottom_off_speech_bubble.transform.position = temp;

			//print(temp);
			yield return null;
		}

		//Swap out speech bubbles
		//Swap out top
		top_speech_bubble.transform.position = dialogue_top.transform.position;
		bubble1.sprite = bubble2.sprite;
		text1.text = text2.text;

		//Swap out bottom
		bottom_speech_bubble.transform.position = dialogue_bottom.transform.position;
		bubble2.sprite = bubble3.sprite;
		text2.text = text3.text;

		//Render next bubble off screen
		bottom_off_speech_bubble.transform.position = dialogue_bottom_off.transform.position;

		transitioning = false;
	}

	// Use this for initialization
	void Start () {
		//Initialize variables.
		dialogue_top = transform.FindChild("dialogue_top").gameObject;
		dialogue_bottom = transform.FindChild("dialogue_bottom").gameObject;
		dialogue_bottom_off = transform.FindChild("dialogue_bottom_off").gameObject;
		dialogue_top_off = transform.FindChild("dialogue_top_off").gameObject;
		top_speech_bubble = dialogue_top.transform.FindChild("top_speech_bubble").gameObject;
		top_off_speech_bubble = dialogue_top_off.transform.FindChild("top_off_speech_bubble").gameObject;
		bottom_speech_bubble = dialogue_bottom.transform.FindChild("bottom_speech_bubble").gameObject;
		bottom_off_speech_bubble = dialogue_bottom_off.transform.FindChild("bottom_off_speech_bubble").gameObject;
		left_sprite = transform.FindChild("left_sprite").gameObject;
		right_sprite = transform.FindChild("right_sprite").gameObject;

		text1 = top_speech_bubble.GetComponent<Text>();
		text2 = bottom_speech_bubble.GetComponent<Text>();
		text3 = bottom_off_speech_bubble.GetComponent<Text>();
		bubble1 = top_speech_bubble.GetComponent<SpriteRenderer>();
		bubble2 = bottom_speech_bubble.GetComponent<SpriteRenderer>();
		bubble3 = bottom_off_speech_bubble.GetComponent<SpriteRenderer>();
		left_sprite.GetComponent<SpriteRenderer> ().sprite = Sprite1;
		right_sprite.GetComponent<SpriteRenderer> ().sprite = Sprite2;

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
		print ("Speakers: " + speaker1 + " and " + speaker2);
		numLines = int.Parse(temp[2]);

		string[] words;
		char[] delimiter = {':'};

		dialogueLines = new dialogueLine[numLines];

		int i = 0;
		foreach (var inp_ln in Lines.Skip(1))
		{
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
			//bottom_speech_bubble.transform.eulerAngles = new Vector3(0, 180, 0);
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
			if(NextScene != ""){
				Application.LoadLevel(NextScene);
			}
		}
		//Check if the user has advanced the dialogue.
		else if((lastRendered != index) && !transitioning){
			StartCoroutine(MoveSpeechBubbles());

			//Render the next line of dialogue offscreen.
			text3.text = dialogueLines[index].dialogue;
			bubble3.sprite = bubble1.sprite;

			/*
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
				//bottom_speech_bubble.transform.eulerAngles = temp;
			}
			else{
				temp.y = 0;
				//bottom_speech_bubble.transform.eulerAngles = temp;
			}
			*/

			lastRendered++;
		}
	}
}
