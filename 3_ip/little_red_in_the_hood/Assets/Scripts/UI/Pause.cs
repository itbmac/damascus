using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour 
{
	Image pauseImage;
	
	// Use this for initialization
	void Start () {
		pauseImage = GetComponent<Image>();
		pauseImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Space))
		{
			pauseImage.enabled = !pauseImage.enabled;
			TheGameManager.Instance.GamePaused = pauseImage.enabled;
		}
	}
}
