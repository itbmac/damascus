using UnityEngine;
using System.Collections;

public class GUIMenuButton : MonoBehaviour
{
		public Texture2D texNor;				// texture normal
		public Texture2D texOver;			// texture mouse click
		public int BType;								// button type
		float timer = 0.0f;								// timer
		bool runTimer = false;						// timer on
		Game game;										// script reference
		
	Touch[] touch;
		// Update is called once per frame
		void Start(){
			guiTexture.texture = texNor;			// set texture
			game = GameObject.Find("Main Camera").GetComponent<Game>();		// get script reference
		}
		
		void Update ()
		{
			if(runTimer){
				if(timer < 0.125f){
					timer += 1*Time.deltaTime;			// run timer
				}else{
					timer = 0.0f;										// reset timer
					runTimer = false;								// set imer off
					guiTexture.texture = texNor;		// set texture
					CallFunction();									// call this function
				}
			}else{
#if UNITY_STANDALONE || UNITY_WEBPLAYER	
				if(Input.GetMouseButtonUp(0)){
					if(guiTexture.HitTest(Input.mousePosition)){
						guiTexture.texture = texOver;			// set texture
						if(BType == 2){
							game.gen.SetActive(true);				// enable game object
						}
						game.PlayClick();									// play sound
						runTimer = true;										// run timer
					}
				}
#endif
#if UNITY_ANDROID
			touch = Input.touches;
			if(touch.Length > 0){
				if(touch[0].phase == TouchPhase.Ended){
					if(guiTexture.HitTest(touch[0].position)){
						guiTexture.texture = texOver;			// set texture
						if(BType == 2){
							game.gen.SetActive(true);				// enable game object
						}
						game.PlayClick();									// play sound
						runTimer = true;										// run timer
					}
				}
			}
#endif
		}
	}
		
		void CallFunction(){
			switch(BType){										// select button type
				case 1:			// clear
					game.DoClear();							// clear game fields
					break;
				case 2:			// new
					Application.LoadLevel(4);			// create new game
					break;
				case 3:			// quit
					Application.LoadLevel(0);			// quit game
					break;
				case 4:			// solve
					game.DoSolve();							// solve puzzle
					break;
			}
		}
}

