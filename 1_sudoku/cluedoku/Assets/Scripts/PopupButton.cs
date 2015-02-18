using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class PopupButton : MonoBehaviour {

	void OnMouseDown() {
		SendMessageUpwards("PopupButtonClicked", name);
	}
}
