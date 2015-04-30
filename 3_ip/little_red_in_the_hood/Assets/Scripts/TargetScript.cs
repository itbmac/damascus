using UnityEngine;
using System.Collections;

public class TargetScript : MyMonoBehaviour {

	public bool isCurrentTarget = false;
	public bool hasBeenPassed = false;

	// Use this for initialization
	void Start () {
		spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetAsCurrentTarget(bool isCurrent) {
		isCurrentTarget = isCurrent;
		hasBeenPassed = !isCurrent;
		
		spriteRenderer.enabled = isCurrent;
	}

	void OnTriggerStay2D(Collider2D other) {
		if ((other.tag == "Player") && isCurrentTarget && (this.name != "William")) {
			SetAsCurrentTarget(false);
		}
	}
}
