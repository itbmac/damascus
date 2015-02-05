using UnityEngine;
using System.Collections;

public class ToLevel : MonoBehaviour {
	public int level = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if(other.name == "Roomba")Application.LoadLevel(level);
	}
}
