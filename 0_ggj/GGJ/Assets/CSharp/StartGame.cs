using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.SetResolution (800, 600, false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){ // if left button pressed...
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				//Switch scene
				Application.LoadLevel(1);
			}
		}
	}
}
