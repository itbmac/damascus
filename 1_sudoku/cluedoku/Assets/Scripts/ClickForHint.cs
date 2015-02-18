using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickForHint : MonoBehaviour {

	public List<Sprite> SpriteStates = new List<Sprite>();
	public AudioClip HintNoise;
	public GameObject board;
	public int remainingNumHints;
	private SpriteRenderer sr;
	
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
			board.GetComponent<BoardManager>().ShakeInvalidTiles();
			remainingNumHints--;
			sr.sprite = SpriteStates[remainingNumHints];
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
