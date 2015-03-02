using UnityEngine;
using System.Collections;

public class SceneSwitchHack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("0"))
			Application.LoadLevel (0);
		if (Input.GetKeyDown("1"))
			Application.LoadLevel (1);
		if (Input.GetKeyDown("2"))
			Application.LoadLevel (2);
		if (Input.GetKeyDown("3"))
			Application.LoadLevel (3);
	}
}
