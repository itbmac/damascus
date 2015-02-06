using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Finish"))
		{
			if (obj.name == "Sphere")
			{
				Debug.Log("did");
				Color c = obj.renderer.material.color;
				c.a = 0.0f;
				obj.renderer.material.color = c;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
