using UnityEngine;
using System.Collections;

public class TargetScript : MyMonoBehaviour {

	public bool isCurrentTarget = false;
	public bool hasBeenPassed = false;
	public bool targetMustRemainOnScreen = false;
	public float targetMustRemainOnScreenTimeBuffer = 4f;
	public float clipDistance = .75f;
	public float timeLastOnScreen;
	public bool renderSprite = true;

	// Use this for initialization
	void Start () {
		spriteRenderer.enabled = false;
		collider2D.isTrigger = true;
	}

	bool gameOver;

	// Update is called once per frame
	void Update () {
		if (isCurrentTarget && targetMustRemainOnScreen) {
			Vector2 tPos = this.transform.position;
			if (Vector2.Distance(Camera.main.WorldToViewportPoint(tPos), new Vector2(.5f, .5f)) < clipDistance) {
				timeLastOnScreen = Time.time;
				FindObjectOfType<FadeToBlack>().image.enabled = false;
				var color = FindObjectOfType<FadeToBlack>().image.color;
				color.a = 0.0f;
				FindObjectOfType<FadeToBlack>().image.color = color;
			}
			else if (!gameOver && timeLastOnScreen + targetMustRemainOnScreenTimeBuffer <= Time.time) {
				gameOver = true;
				FindObjectOfType<FadeToBlack>().Trigger();
			}
			else {
				FindObjectOfType<FadeToBlack>().image.enabled = true;
				var color = FindObjectOfType<FadeToBlack>().image.color;
				color.a = ((Time.time - timeLastOnScreen) / targetMustRemainOnScreenTimeBuffer);
				FindObjectOfType<FadeToBlack>().image.color = color;
			}
		}
	}

	public void SetAsCurrentTarget(bool isCurrent) {
		isCurrentTarget = isCurrent;
		hasBeenPassed = !isCurrent;
		
		spriteRenderer.enabled = renderSprite && isCurrent;
	}

	void OnTriggerStay2D(Collider2D other) {
		if ((other.tag == "Player") && isCurrentTarget && (this.name != "William")) {
			SetAsCurrentTarget(false);
		}
	}
}
