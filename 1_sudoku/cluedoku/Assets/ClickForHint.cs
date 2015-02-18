using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickForHint : MonoBehaviour {

	public List<Sprite> SpriteStates = new List<Sprite>();
	public AudioClip HintNoise;
	private int remainingNumHints;
	private SpriteRenderer sr;
	public HintPopup hintPopup;
	
	public int RemainingNumHints {
		set {
			remainingNumHints = value;
			sr.sprite = SpriteStates[remainingNumHints];
		}
		
		get {
			return remainingNumHints;
		}
	}
	
	void Awake() {
		hintPopup = FindObjectOfType<HintPopup>();
	}
	
	// Use this for initialization
	void Start () {
		sr = (SpriteRenderer)renderer;
		
		Reset();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnMouseDown() {
		if (remainingNumHints > 0) {
			hintPopup.Trigger();
			audio.PlayOneShot(HintNoise);
		}
	}

	void OnMouseEnter() {
		if (remainingNumHints > 0) {
			sr.sprite = SpriteStates[remainingNumHints-1];
		}
	}
	
	void OnMouseExit() {
		if (remainingNumHints > 0) {
			sr.sprite = SpriteStates[remainingNumHints];
		}
	}
	
	public void Reset() {
		remainingNumHints = SpriteStates.Count - 1;
		sr.sprite = SpriteStates[remainingNumHints];
	}
}
