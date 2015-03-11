using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class LoadLevelButton : MonoBehaviour {

	public string TargetLevelName;

	void OnMouseDown() {
		Application.LoadLevel(TargetLevelName);
	}
}
