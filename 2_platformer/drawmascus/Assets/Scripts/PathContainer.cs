using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PathContainer : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		int i = 0;
		foreach (Transform child in transform) {
			i += 1; 
			child.name = name + " " + i.ToString();
		}
	
	}
}
