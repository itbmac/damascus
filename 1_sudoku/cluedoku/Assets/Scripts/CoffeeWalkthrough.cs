using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoffeeWalkthrough : MonoBehaviour {

	public List<Sprite> SpriteStates = new List<Sprite>();
	public GameObject HintPopUpObj;
	public int currentStateNum;
	private SpriteRenderer sr;
	private bool prevPopUpRenderState = false;

	// Use this for initialization
	void Start () {
		currentStateNum = 0;
		sr = (SpriteRenderer)renderer;
		sr.sprite = SpriteStates[currentStateNum];
		prevPopUpRenderState = false;
	}
	
	// Update is called once per frame
	void Update () {
		if ((currentStateNum == 0) && !prevPopUpRenderState && HintPopUpObj.activeSelf) {
			Advance();
			prevPopUpRenderState = true;
		}
		if ((currentStateNum == 1) && prevPopUpRenderState && !HintPopUpObj.activeSelf) {
			Advance();
			prevPopUpRenderState = false;
		}
		if ((currentStateNum == 2) && !prevPopUpRenderState && HintPopUpObj.activeSelf) {
			Advance();
			prevPopUpRenderState = true;
		}
		if ((currentStateNum == 3) && prevPopUpRenderState && !HintPopUpObj.activeSelf) {
			gameObject.SetActive(false);
		}
	}

	void Advance() {
		currentStateNum++;
		sr.sprite = SpriteStates[currentStateNum];
	}
}
