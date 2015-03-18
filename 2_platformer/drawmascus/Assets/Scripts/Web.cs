using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Web : ColorToggle {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected override void OnTurnReal() {
		
	}
	
	protected override void OnTurnDrawing() {
		
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.name == "Butterfly" && other.attachedRigidbody.velocity != Vector2.zero)
			other.SendMessage("Ensnare");
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Butterfly")
			other.SendMessage("Ensnare");
	}
}
