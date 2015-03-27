using UnityEngine;
using System.Collections;

public class GrayFeather : Colorable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnTurnReal() {
		rigidbody2D.isKinematic = false;
	}
	
	public override void OnTurnDrawing() {
		rigidbody2D.isKinematic = true;
	}
}
