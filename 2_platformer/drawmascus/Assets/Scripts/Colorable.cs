using UnityEngine;
using System.Collections;

public abstract class Colorable : MonoBehaviour {

	protected abstract void Activate();
	protected abstract void Deactivate();
	
	// TODO: use
	private void OnActivate() {
		collider2D.enabled = true;
		
		Activate();
	}
	
	// TODO: use
	private void OnDeactivate() {
		collider2D.enabled = false;
	
		Deactivate();
	}
}
