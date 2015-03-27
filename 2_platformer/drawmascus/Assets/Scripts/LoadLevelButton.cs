using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class LoadLevelButton : MonoBehaviour {

	public string TargetLevelName;

	void OnMouseDown() {
		if (TargetLevelName == "Quit")
			Application.Quit();
		Application.LoadLevel(TargetLevelName);
	}

	void OnMouseEnter() {
		transform.parent.renderer.enabled = true;
	}
	
	void OnMouseExit() {
		transform.parent.renderer.enabled = false;
	}
}
