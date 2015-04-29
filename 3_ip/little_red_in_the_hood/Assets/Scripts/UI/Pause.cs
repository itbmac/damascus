using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour 
{
	Image pauseImage;
	
	bool _paused;
	bool Paused {
		get {
			return _paused;
		}
		
		set {
			_paused = value;
			if (_paused)
			{
				pauseImage.enabled = true;
				Time.timeScale = 0;
			}
			else
			{				
				pauseImage.enabled = false;
				Time.timeScale = 1;	
			}
		}
	}
	
	
	// Use this for initialization
	void Start () {
		pauseImage = GetComponent<Image>();
		pauseImage.enabled = false;
		Paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Space))
			Paused = !Paused;
	}
}
