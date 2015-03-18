using UnityEngine;
using System.Collections;

public class TriggerNextLevel : MonoBehaviour {

	void Start() {
		var sr = (SpriteRenderer)renderer;
		sr.enabled = false;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Wolf")
			Application.LoadLevel(Application.loadedLevel + 1);
	}
}
