using UnityEngine;
using System.Collections;

public class ClickToNextScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
			Cursor.visible = true;
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	
	}
}
