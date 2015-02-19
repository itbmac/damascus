using UnityEngine;
using System.Collections;

public class ClickToRestart : MonoBehaviour {

	public GameObject board;
	public AudioClip clearNoise;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown() {
		if (renderer.enabled) {
			audio.PlayOneShot(clearNoise);
			board.GetComponent<BoardManager>().RecallTiles();
		}
	}
}
