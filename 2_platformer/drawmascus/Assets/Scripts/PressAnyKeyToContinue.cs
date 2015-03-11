using UnityEngine;
using System.Collections;

public class PressAnyKeyToContinue : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {	
		if (Input.anyKeyDown) {
			if (Application.loadedLevel == Application.levelCount - 1)
				Application.LoadLevel(0);
			else
				Application.LoadLevel(Application.loadedLevel + 1);
		}
	}
}
