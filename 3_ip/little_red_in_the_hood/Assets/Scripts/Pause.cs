using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour 
{
	bool visible;
	Image pauseImage;
	
	// Use this for initialization
	void Start () {
		visible = false;
		pauseImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Space))
		{
			visible = !visible;
		}
		
		if (visible)
		{
			pauseImage.enabled = true;
		}
		else
		{
			pauseImage.enabled = false;
		}
	}
}
