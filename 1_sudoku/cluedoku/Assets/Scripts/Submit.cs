using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Submit : MonoBehaviour {

	private BoardManager manager;
	public Sprite spriteDisabled;
	public Sprite spriteEnabled;
	
	void Start() {
		manager = FindObjectOfType<BoardManager>();
	}

	void OnMouseDown() {
		manager.SendMessage("Submit");
	}
}
