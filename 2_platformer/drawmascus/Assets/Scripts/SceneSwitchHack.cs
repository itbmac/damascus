using UnityEngine;
using System.Collections;

public class SceneSwitchHack : MonoBehaviour {
	const int numLevels = 4;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < numLevels; i++){
			if (Input.GetKeyDown(i.ToString()))
				Application.LoadLevel (i);
		}
	}
}
