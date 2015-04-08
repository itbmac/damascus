using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialogue : MonoBehaviour {
	//Current line index being read.
	int index = 0;
	//The last line index that was visually rendered.
	int lastRendered = -1;
	string[] speech = new string[] {"test1", "test2", "test3", "test4", "test5", "test6"};

	//Will initialize these privately later
	public GameObject dialogue_top;
	public GameObject dialogue_bottom;
	public GameObject dialogue_bottom_off;
	public GameObject dialogue_top_off;

	Text text1, text2, text3, text4;

	// Use this for initialization
	void Start () {
		//Initialize variables.
		text1 = dialogue_bottom_off.GetComponent<Text>();
		text2 = dialogue_bottom.GetComponent<Text>();
		text3 = dialogue_top.GetComponent<Text>();
		text4 = dialogue_top_off.GetComponent<Text>();

		//Set background.

		//Read in dialogue file.

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space") && (lastRendered == index)){
			index++;
		}
		
		//If the user advances the dialogue but there's none left, go to the next act.
		//for now, this doesn't do anything.
		if(index == speech.Length){
			//advance to next scene
		}
		//Check if the user has advanced the dialogue.
		else if(lastRendered != index){
			//Render the next line of dialogue offscreen.
			text1.text = speech[index];

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
