using UnityEngine;
using System.Collections;

public class TriggerNextLevel : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Wolf")
			Application.LoadLevel(Application.loadedLevel + 1);
	}
}
