using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickForHint : MonoBehaviour {

	public List<Sprite> SpriteStates = new List<Sprite>();
	public AudioClip HintNoisePopUp, HintNoiseUse, HintNoiseCancel;
	private int remainingNumHints;
	public GameObject board;
	public int startingNumHints;
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
			audio.PlayOneShot(HintNoisePopUp);
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

	public void UseAHint() {
		remainingNumHints--;
		sr.sprite = SpriteStates[remainingNumHints];
		audio.PlayOneShot(HintNoiseUse);
	}

	public void CancelHintUse() {
		audio.PlayOneShot(HintNoiseCancel);
	}
	
	public void Reset() {
		remainingNumHints = startingNumHints; //SpriteStates.Count - 1;
		sr.sprite = SpriteStates[remainingNumHints];
	}
}
