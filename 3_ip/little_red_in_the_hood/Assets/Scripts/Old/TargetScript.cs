using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {

	public bool isCurrentTarget = false;
	public bool hasBeenPassed = false;

	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = this.GetComponent("SpriteRenderer") as SpriteRenderer;
		spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetAsCurrentTarget(bool isCurrent) {
		isCurrentTarget = isCurrent;
		hasBeenPassed = !isCurrent;

		if (spriteRenderer == null) {
			spriteRenderer = this.GetComponent("SpriteRenderer") as SpriteRenderer;
		}
		spriteRenderer.enabled = isCurrent;
	}

	void OnTriggerStay2D(Collider2D other) {
		if ((other.tag == "Car") && isCurrentTarget) {
			SetAsCurrentTarget(false);
		}
	}
}
