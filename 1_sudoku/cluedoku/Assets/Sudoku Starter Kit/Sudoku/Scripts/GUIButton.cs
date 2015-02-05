using UnityEngine;
using System.Collections;

public class GUIButton : MonoBehaviour {

	public Texture2D en;				// texture enabled
	public Texture2D dis;			// texture disabled
	public bool enable;				// enable button
	public int number;					// number 
	public bool special;				// is special number
	
	Game game;							// game script reference

	Touch[] touch;

	void Start(){
		game = GameObject.Find("Main Camera").GetComponent<Game>();		// get script reference
	}
	
	void Update () {
			if(!special){										// not special button
				if(game.selected == null){			// no field selected
					enable = false;							// enable button
				}
				
				if(enable){																// button enabled
					guiTexture.texture = en;										// set texture
#if UNITY_STANDALONE || UNITY_WEBPLAYER
					if(Input.GetMouseButtonUp(0)){
						if(guiTexture.HitTest(Input.mousePosition)){
							game.PlayClick();											// play sound
							game.selected.renderer.material.mainTexture = game.num[number];		// set texture
							game.selected.GetComponent<Field>().value = number;								// set value
							game.selected = null;																									// remove selection
						}
					}
#endif
#if UNITY_ANDROID
				touch = Input.touches;
				if(touch.Length > 0){
					if(touch[0].phase == TouchPhase.Ended){
						if(guiTexture.HitTest(touch[0].position)){
							game.PlayClick();											// play sound
							game.selected.renderer.material.mainTexture = game.num[number];		// set texture
							game.selected.GetComponent<Field>().value = number;								// set value
							game.selected = null;																									// remove selection
						}
					}
				}
#endif
				}else{
					guiTexture.texture = dis;								// set texture
				}
			}else{
#if UNITY_STANDALONE || UNITY_WEBPLAYER
				if(Input.GetMouseButtonUp(0)){
					if(guiTexture.HitTest(Input.mousePosition)){
						game.PlayClick();										// 
						game.SwitchMenu();								// change ingame menu
					}
				}
#endif
#if UNITY_ANDROID
				touch = Input.touches;
				if(touch.Length > 0){
					if(touch[0].phase == TouchPhase.Ended){
						if(guiTexture.HitTest(touch[0].position)){
							game.PlayClick();										// 
							game.SwitchMenu();								// change ingame menu
						}
					}
				}
#endif
			}
	}
}
