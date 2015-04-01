using UnityEngine;
using System.Collections;

public class TrackLinePassed : MonoBehaviour {
	public enum possibleVelocity{ANY, NEGATIVE, POSITIVE, ZERO};

	public int lineNumber = 0;
	public bool lineIsStart = true;
	public bool lineIsEnd = false;
	public bool linePassed = false;
	public string lineNameBase= "Track_Line";

	public possibleVelocity desiredYVelocity;
	public possibleVelocity desiredXVelocity;

	public Sprite lineGraphicNext;
	public Sprite lineGraphicEnd;

	// Use this for initialization
	void Start () {
		if (lineIsStart) {
			SpriteRenderer sprRenderer = this.GetComponent("SpriteRenderer") as SpriteRenderer;
			sprRenderer.sprite = lineGraphicNext;
			sprRenderer.enabled = true;
		}
		else if (lineIsEnd) {
			SpriteRenderer sprRenderer = this.GetComponent("SpriteRenderer") as SpriteRenderer;
			sprRenderer.sprite = lineGraphicEnd;
		}
		else {
			SpriteRenderer sprRenderer = this.GetComponent("SpriteRenderer") as SpriteRenderer;
			sprRenderer.sprite = lineGraphicNext;
			sprRenderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		bool passThruFail = false;
		Rigidbody2D body = other.attachedRigidbody;

		if (!passThruFail && (desiredXVelocity == possibleVelocity.POSITIVE)) {
			if (body.velocity.x <= 0.0f) {
				passThruFail = true;
			}
		}

		if (!passThruFail && (desiredXVelocity == possibleVelocity.NEGATIVE)) {
			if (body.velocity.x >= 0.0f) {
				passThruFail = true;
			}
		}

		if (!passThruFail && (desiredYVelocity == possibleVelocity.POSITIVE)) {
			if (body.velocity.y <= 0.0f) {
				passThruFail = true;
			}
		}

		if (!passThruFail && (desiredYVelocity == possibleVelocity.NEGATIVE)) {
			if (body.velocity.y >= 0.0f) {
				passThruFail = true;
			}
		}

		if (!passThruFail && !linePassed) {
			if (!lineIsStart) {
				GameObject objPrev = GameObject.Find(lineNameBase + (lineNumber-1).ToString());
				if (objPrev) {
					TrackLinePassed lineEndChecker = objPrev.GetComponent("TrackLinePassed") as TrackLinePassed;
					if (lineEndChecker && lineEndChecker.linePassed) {
						linePassed = true;
					}
				}
			}
			else {
				linePassed = true;
			}

			if (linePassed && !lineIsEnd) {
				GameObject objNext = GameObject.Find(lineNameBase + (lineNumber+1).ToString());
				SpriteRenderer sprRendererObj = objNext.GetComponent("SpriteRenderer") as SpriteRenderer;
				sprRendererObj.enabled = true;

				SpriteRenderer sprRenderer = this.GetComponent("SpriteRenderer") as SpriteRenderer;
				sprRenderer.enabled = false;
			}
			else if (linePassed && lineIsEnd) {
				GameObject objNext = GameObject.Find(lineNameBase + (0).ToString());
				SpriteRenderer sprRendererObj = objNext.GetComponent("SpriteRenderer") as SpriteRenderer;
				sprRendererObj.enabled = true;
			}
		}
	}
}
