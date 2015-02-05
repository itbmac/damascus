using UnityEngine;
using System.Collections;

public class GUIMMButton : MonoBehaviour
{
	public Texture2D texNor;					// normal texture
	public Texture2D texOver;				// click texture
	public int BType;									// button type
	float timer = 0.0f;									// timer
	bool runTimer = false;							// timer on
	public AudioClip clickSound;			// click sound

	Touch[] touch;

	// Update is called once per frame
	void Start(){
		guiTexture.texture = texNor;		// set texture
	}	
	
	void Update (){
		string svol = PlayerPrefs.GetString("soundvolume","on");		// get sound volume
		
		if(svol == "on"){
			audio.volume = 1.0f;				// set sound volume
		}else{
			audio.volume = 0.0f;
		}
		
		if(runTimer){
			if(timer < 0.125f){
				timer += 1*Time.deltaTime;				// count time
			}else{
				timer = 0.0f;												// reset timer
				runTimer = false;										// timer off
				guiTexture.texture = texNor;				// set texture
				audio.PlayOneShot(clickSound);			// play sound
				CallFunction();											// call this function
			}
		}else{
#if UNITY_STANDALONE || UNITY_WEBPLAYER	
			if(Input.GetMouseButtonUp(0)){
				if(guiTexture.HitTest(Input.mousePosition)){		
					guiTexture.texture = texOver;			// set texture
					runTimer = true;										// timer on
				}
			}
#endif
#if UNITY_ANDROID
			touch = Input.touches;
			if(touch.Length > 0){
				if(touch[0].phase == TouchPhase.Ended){
					if(guiTexture.HitTest(touch[0].position)){
						guiTexture.texture = texOver;
						runTimer = true;
					}
				}
			}
#endif
		}
	}
	
	void CallFunction(){
		int lNo = 0;							// level number
		switch(BType){
			case 1:			// new
				lNo = 1;							// set level number
				break;
			case 2:			// options
				lNo = 2;
				break;
			case 3:			// scores
				lNo = 3;
				break;
			case 4:			// quit
				Application.Quit();
				break;
		}
		Application.LoadLevel(lNo);			// load level
	}
}

