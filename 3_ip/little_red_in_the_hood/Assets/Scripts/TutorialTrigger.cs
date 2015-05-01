using UnityEngine;
using System.Collections;

public class TutorialTrigger : MyMonoBehaviour {

	public string TextToTrigger = "";

	// Use this for initialization
	void Start () {
		spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") {
			TutorialText.Instance.TriggerText(TextToTrigger);
		}
	}
}
