using UnityEngine;
using System.Collections;

public class PressAnyKeyToContinue : MonoBehaviour {

	public string DestLevel;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
			if (DestLevel != "") {
				Application.LoadLevel(DestLevel);
				return;
			}			
		
			if (Application.loadedLevel == Application.levelCount - 1)
				Application.LoadLevel(0);
			else
				Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
