using UnityEngine;
using System.Collections;

public class RestartKey : MonoBehaviour {

	public string Key = "r";
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(Key))
			Application.LoadLevel(Application.loadedLevel);
	}
}
