using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		else if (Input.GetKeyDown(KeyCode.R))
			Application.LoadLevel(Application.loadedLevel);
			
#if UNITY_EDITOR
		// for cheats that only work in the editor
		

#endif
	}
}
