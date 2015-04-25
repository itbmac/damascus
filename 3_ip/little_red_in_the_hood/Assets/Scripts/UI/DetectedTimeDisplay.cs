using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DetectedTimeDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (TheGameManager.Instance.DetectedTimeRemaining > 0) {
			GetComponent<Text>().text = "";// "DETECTED\n" + TheGameManager.Instance.DetectedTimeRemaining.ToString("0.0");
		} else {
			GetComponent<Text>().text = "";
		}
	}
}
