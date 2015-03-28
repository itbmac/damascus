using UnityEngine;
using System.Collections;

public class ClickToNextScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
			Application.LoadLevel(Application.loadedLevel + 1);
	
	}
}
