using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
public class SimpleColorToggle : MonoBehaviour, Colorable {
	// doesn't do anything special, just turns real or drawing
	// based on the assigned sprites, and sets layer accordingly

	public void OnTurnReal() {
	
	}
	
	public void OnTurnDrawing() {
	
	}
}
