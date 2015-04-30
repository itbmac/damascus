/*
 * DIALOGUE SYSTEM
 * 
 * Notes:
 * 	-currently only supports 2 speakers at a time. speakers can be swapped out.
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

struct dialogueLine {
	public string character;
	public string dialogue;
	public string mood;

	public dialogueLine(string[] s){
		character = s[0];
		dialogue = s[1];
		if (s.Length <= 2)
			mood = "normal";
		else
			mood = s[2];
	}

	public dialogueLine(string command){
		character = command;
		dialogue = "";
		mood = "";
	}
}

public class Dialogue : MonoBehaviour {
	//Dictionary of assets.
	Dictionary<string, Sprite> assets = new Dictionary<string, Sprite> ();

	//Current line index being read.
	int index = 0;
	//The last line index that was visually rendered.
	int lastRendered = 0;

	List<dialogueLine> dialogueLines;

	GameObject dialogue_top, dialogue_bottom, dialogue_bottom_off, dialogue_top_off;
	GameObject top_speech_bubble, top_off_speech_bubble, bottom_speech_bubble, bottom_off_speech_bubble;
	SpriteRenderer left_sprite, right_sprite;

	//The text file we read from.
	public TextAsset MyText;

	//Sprites
	//sprite1 corresponds with speaker1, sprite2 corresponds with speaker2.
	//speaker1 on the left, speaker2 on the right.
	GameObject left, right, offscreen;
	Sprite sprite1, sprite2, sprite3, sprite4;
	string speaker1 = "none";
	string speaker2 = "none";
	string speaker3 = "none"; 
	string speaker4 = "none";
	bool currentlyFirstSpeakers = true;
	string currentSpeaker1, currentSpeaker2;

	Text text1, text2, text3;
	SpriteRenderer bubble1, bubble2, bubble3;

	//The next scene after this dialogue.
	public string NextScene;
	
	string[] Lines;

	//Basically a mutex so the player can't skip through text.
	bool transitioning = false;

	//Compile dictionary.
	void buildAssetDictionary(){
		Sprite[] temp = Resources.LoadAll<Sprite>("Dialogue");

		for(int i = 0; i < temp.Length; i++){
			assets.Add(temp[i].name, temp[i]);
		}
	}

	//Transitioning speakers.
	IEnumerator SwapSpeakers() {
		transitioning = true;

		float distance1 = Vector3.Distance(offscreen.transform.position, left.transform.position);
		float distance2 = Vector3.Distance(offscreen.transform.position, right.transform.position);
		float speed = 0.03f;

		Vector3 temp;

		//Erase speech bubbles.
		bubble1.sprite = null;
		bubble2.sprite = null;
		text1.text = "";
		text2.text = "";

		//Transition out current speakers.
		for (float f = 1f; f >= 0; f -= speed) {
			temp = left_sprite.transform.position;
			temp.x += distance1 * speed;
			left_sprite.transform.position = temp;

			temp = right_sprite.transform.position;
			temp.x += distance2 * speed;
			right_sprite.transform.position = temp;
			yield return null;
		}

		//Swap out speakers.
		if (currentlyFirstSpeakers) {
			left_sprite.sprite = sprite3;
			right_sprite.sprite = sprite4;
			currentSpeaker1 = speaker3;
			currentSpeaker2 = speaker4;
		} 
		else {
			left_sprite.sprite = sprite1;
			right_sprite.sprite = sprite2;
			currentSpeaker1 = speaker1;
			currentSpeaker2 = speaker2;
		}
		currentlyFirstSpeakers = !currentlyFirstSpeakers;

		//Transition in next speakers.
		for (float f = 1f; f >= 0; f -= speed) {
			temp = left_sprite.transform.position;
			temp.x -= distance1 * speed;
			left_sprite.transform.position = temp;
			
			temp = right_sprite.transform.position;
			temp.x -= distance2 * speed;
			right_sprite.transform.position = temp;
			yield return null;
		}

		left_sprite.transform.localPosition = Vector3.zero;
		right_sprite.transform.localPosition = Vector3.zero;

		transitioning = false;
	}

	//Transitioning speech bubbles.
	IEnumerator MoveSpeechBubbles() {
		transitioning = true;
		float distance1, distance2, distance3;
		float speed = 0.05f;

		distance1 = Vector3.Distance(top_off_speech_bubble.transform.position, top_speech_bubble.transform.position);
		distance2 = Vector3.Distance(top_speech_bubble.transform.position, bottom_speech_bubble.transform.position);
		distance3 = Vector3.Distance(bottom_speech_bubble.transform.position, bottom_off_speech_bubble.transform.position);

		Vector3 temp;

		for (float f = 1f; f >= 0; f -= speed) {
			//top to top-off
			temp = top_speech_bubble.transform.position;
			temp.y += distance1 * speed;
			top_speech_bubble.transform.position = temp;

			//bottom to top
			temp = bottom_speech_bubble.transform.position;
			temp.y += distance2 * speed;
			bottom_speech_bubble.transform.position = temp;

			//bottom-off to bottom
			temp = bottom_off_speech_bubble.transform.position;
			temp.y += distance3 * speed;
			bottom_off_speech_bubble.transform.position = temp;

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

		left = transform.FindChild("left_sprite").gameObject;
		right = transform.FindChild("right_sprite").gameObject;
		offscreen = transform.FindChild("offscreen").gameObject;
		left_sprite = transform.FindChild("left_sprite/sprite").gameObject.GetComponent<SpriteRenderer> ();
		right_sprite = transform.FindChild("right_sprite/sprite").gameObject.GetComponent<SpriteRenderer> ();

		text1 = top_speech_bubble.GetComponent<Text>();
		text2 = bottom_speech_bubble.GetComponent<Text>();
		text3 = bottom_off_speech_bubble.GetComponent<Text>();
		bubble1 = top_speech_bubble.GetComponent<SpriteRenderer>();
		bubble2 = bottom_speech_bubble.GetComponent<SpriteRenderer>();
		bubble3 = bottom_off_speech_bubble.GetComponent<SpriteRenderer>();

		//Build asset dictionary.
		buildAssetDictionary();

		//Set background.

		//Read in dialogue file.
		Lines = MyText.text.Split (new char[] {'\r', '\n'}, System.StringSplitOptions.RemoveEmptyEntries);

		//Read the first line, parse, and use this information.
		string inp_ln_first = Lines[0];

		// The first line indicates the two speakers.
		char[] delin = {' '};
		string[] temp = inp_ln_first.Split(delin);
		speaker1 = temp[0].ToLower();
		if(temp.Length > 1)
			speaker2 = temp[1].ToLower();

		//Read the second line, parse, and use this information.
		string inp_ln_second = Lines[1];
		
		// The second line indicates the next two speakers, if they exist.
		temp = inp_ln_second.Split(delin);
		if(temp.Length > 0)
			speaker3 = temp[0].ToLower();
		if(temp.Length > 1)
			speaker4 = temp[1].ToLower();

		currentSpeaker1 = speaker1;
		currentSpeaker2 = speaker2;

		//Get sprites for all of them.
		if(speaker1 != "none") sprite1 = assets[speaker1 + "_normal"];
		if(speaker2 != "none") sprite2 = assets[speaker2 + "_normal"];
		if(speaker3 != "none") sprite3 = assets[speaker3 + "_normal"];
		if(speaker4 != "none") sprite4 = assets[speaker4 + "_normal"];

		left_sprite.sprite = sprite1;
		right_sprite.sprite = sprite2;

		string[] words;
		char[] delimiter = {':'};

		dialogueLines = new List<dialogueLine>();

		int i = 0;
		foreach (var inp_ln in Lines.Skip(2))
		{
			if(inp_ln == "switch"){
				dialogueLines.Add(new dialogueLine(inp_ln));
			}
			else{
				words = inp_ln.Split(delimiter);
				dialogueLines.Add (new dialogueLine(words));
			}
			i++;
		}
		
		//Load the first line of text.
		text2.text = dialogueLines[0].dialogue;
		string speakerMood = (dialogueLines[0].character + "_" + dialogueLines[0].mood).ToLower();
		if (dialogueLines [0].character == speaker2) {
			bubble2.sprite = assets ["normal_right"];
			if (assets.ContainsKey (speakerMood)) {
				right_sprite.sprite = assets [speakerMood];
			}
		} else {
			bubble2.sprite = assets ["normal_left"];
			if (assets.ContainsKey (speakerMood)) {
				left_sprite.sprite = assets [speakerMood];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown ("space") || Input.GetMouseButtonDown(0)) && (lastRendered == index)){
			index++;
		}
		
		//If the user advances the dialogue but there's none left, go to the next act.
		//for now, this doesn't do anything.
		if(index == dialogueLines.Count){
			//advance to next scene
//			if(NextScene != ""){
//				Application.LoadLevel(NextScene);
//			}
			Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
		}
		//Check if the user has advanced the dialogue.
		else if((lastRendered != index) && !transitioning){
			if(dialogueLines[index].character == "switch"){
				//Swap out the set of speakers.
				StartCoroutine(SwapSpeakers());
				//Render the next line of dialogue offscreen.
				index++;
				//text3.text = dialogueLines[index].dialogue;
				//if(dialogueLines[index].character.ToLower() == currentSpeaker2) bubble3.sprite = assets["normal_right"];
				//else bubble3.sprite = assets["normal_left"];
			}

			else{
				//Adjust the next speaker's mood.
				string speakerMood = (dialogueLines[index - 1].character + "_" + dialogueLines[index - 1].mood).ToLower();

				if(assets.ContainsKey(speakerMood)){
					if(dialogueLines[index - 1].character.ToLower() == currentSpeaker1) left_sprite.sprite = assets[speakerMood];
					else right_sprite.sprite = assets[speakerMood];
				}
				//Move speech bubbles.
				StartCoroutine(MoveSpeechBubbles());

				//Render the next line of dialogue offscreen.
				text3.text = dialogueLines[index].dialogue;
				if(dialogueLines[index].character.ToLower() == currentSpeaker2) bubble3.sprite = assets["normal_right"];
				else bubble3.sprite = assets["normal_left"];
			}

			lastRendered++;
		}
	}
}
