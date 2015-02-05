using UnityEngine;
using System.Collections;

public class GUIOButton : MonoBehaviour
{
	public Texture2D texOn;				// normal texture
	public Texture2D texOff;				// click texture
	public int BType;								// button type
	public AudioClip clickSound;		// click sound

	Touch[] touch;


	// Update is called once per frame
	void Start(){
		ButtonSetup();								// set buttons
	}
	
	void Update (){
		CheckVol();										// check volume 
		ButtonSetup();								// 

#if UNITY_STANDALONE || UNITY_WEBPLAYER	
		if(Input.GetMouseButtonUp(0)){
			if(guiTexture.HitTest(Input.mousePosition)){
				CallFunction();						// call this function
			}
		}
#endif
#if UNITY_ANDROID
		touch = Input.touches;
		if(touch.Length > 0){
			if(touch[0].phase == TouchPhase.Ended){
				if(guiTexture.HitTest(touch[0].position)){
					CallFunction();						// call this function
				}
			}
		}
#endif
	}
	
	void CheckVol(){
		string svol = PlayerPrefs.GetString("soundvolume","on");		// get sound volume (on, off)
		
		if(svol == "on"){
			audio.volume = 1.0f;				// set sound volume
		}else{
			audio.volume = 0.0f;
		}
	}
	
	void CallFunction(){
		switch(BType){
			case 1:			// music off
				PlayerPrefs.SetString("musicvolume","off");				// set music volume
				break;
			case 2:			// music on
				PlayerPrefs.SetString("musicvolume","on");
				break;
			case 3:			// sound off
				PlayerPrefs.SetString("soundvolume","off");				// set sound volume
				break;
			case 4:			// sound on
				PlayerPrefs.SetString("soundvolume","on");
				break;
		}
		CheckVol();																			//
		audio.PlayOneShot(clickSound);									// play sound
	}
	
	void ButtonSetup(){
		string mvol = PlayerPrefs.GetString("musicvolume","on");			// get music volume
		string svol = PlayerPrefs.GetString("soundvolume","on");			// get sound volume
		
		switch(BType){																// select button type
			case 1:			// music off
				if(mvol == "on"){
					guiTexture.texture = texOff;							// set textures
				}else{
					guiTexture.texture = texOn;
				}
				break;
			case 2:			// music on
				if(mvol == "on"){
					guiTexture.texture = texOn;
				}else{
					guiTexture.texture = texOff;
				}
				break;
			case 3:			// sound off
				if(svol == "on"){
					guiTexture.texture = texOff;
				}else{
					guiTexture.texture = texOn;
				}
				break;
			case 4:			// sound on
				if(svol == "on"){
					guiTexture.texture = texOn;
				}else{
					guiTexture.texture = texOff;
				}
				break;
		}
	}
}

