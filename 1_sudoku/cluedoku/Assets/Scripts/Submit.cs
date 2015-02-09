using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Submit : MonoBehaviour {

	BoardManager manager;
	
	void Start() {
		manager = FindObjectOfType<BoardManager>();
	}

	void OnMouseDown() {
		manager.SendMessage("Submit");
	}
}
