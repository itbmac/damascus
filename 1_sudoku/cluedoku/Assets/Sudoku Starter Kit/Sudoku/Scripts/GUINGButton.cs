using UnityEngine;
using System.Collections;

public class GUINGButton : MonoBehaviour
{
	public Texture2D texNor;				// normal texture
	public Texture2D texOver;			// click texture
	public int BType;								// button type
	float timer = 0.0f;								// timer
	bool runTimer = false;						// timer on
	
	public GameObject gen;				// generating puzzle GUITexture
	public AudioClip clickSound;		// click sound

	Touch[] touch;

	void Start(){
		guiTexture.texture = texNor;		// set texture
		gen.SetActive(false);						// disable game object
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
				timer += 1*Time.deltaTime;			// count time
			}else{
				timer = 0.0f;										// reset timer
				runTimer = false;								// timer off
				guiTexture.texture = texNor;		// set texture
				CallFunction();									// call this function
			}
		}else{
#if UNITY_STANDALONE || UNITY_WEBPLAYER	
			if(Input.GetMouseButtonUp(0)){
				if(guiTexture.HitTest(Input.mousePosition)){
					guiTexture.texture = texOver;					// set texture
					audio.PlayOneShot(clickSound);				// play sound
					gen.SetActive(true);										// enable game object
					runTimer = true;												// timer on
				}
			}
#endif
#if UNITY_ANDROID
			touch = Input.touches;
			if(touch.Length > 0){
				if(touch[0].phase == TouchPhase.Ended){
					if(guiTexture.HitTest(touch[0].position)){
						guiTexture.texture = texOver;					// set texture
						audio.PlayOneShot(clickSound);				// play sound
						gen.SetActive(true);										// enable game object
						runTimer = true;												// timer on
					}
				}
			}
#endif
		}
	}
	
	void CallFunction(){
		switch(BType){
		case 1:			// easy
			PlayerPrefs.SetString("gamelevel", "easy");				// set game level
			break;
		case 2:			// medium
			PlayerPrefs.SetString("gamelevel", "medium");		//
			break;
		case 3:			// hard
			PlayerPrefs.SetString("gamelevel", "hard");				//
			break;
		}
		
		Application.LoadLevel(4);														// load game
	}
}

