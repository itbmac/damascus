using UnityEngine;
using System.Collections;

public class TimeElapsed : MonoBehaviour {

	public int precision = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		guiText.text = (Mathf.RoundToInt(Time.time)).ToString();
	}
}
