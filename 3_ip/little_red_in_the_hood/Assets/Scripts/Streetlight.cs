using UnityEngine;
using System.Collections;

public class Streetlight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.name == "Player") {
			Player.Instance.IsUnderStreetlight = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D c) {
		if (c.name == "Player") {
			Player.Instance.IsUnderStreetlight = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D c) {
		if (c.name == "Player")
			Player.Instance.IsUnderStreetlight = false;
	}
}
