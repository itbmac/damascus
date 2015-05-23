using UnityEngine;
using System.Collections;

public class HighlightOnHover : MonoBehaviour {

	public Sprite spriteUnhighlighted;
	public Sprite spriteHighlighted;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = (SpriteRenderer)GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		sr.sprite = spriteHighlighted;
	}

	void OnMouseExit() {
		sr.sprite = spriteUnhighlighted;
	}

	void OnMouseDown() {
		sr.sprite = spriteUnhighlighted;
	}
}
