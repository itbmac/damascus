using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

// from https://stackoverflow.com/questions/23152525/drag-object-in-unity-2d
public class Drag : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	private bool wasDragged;
	const float SIZE = 2.5f;
	
	void Start() {
		Snap ();
	}
	
	void OnMouseDown() {
		
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
		wasDragged = true;
	}
	
	private float Adjust(float f) {
		return (Mathf.Round(.5f + f / SIZE) - .5f) * SIZE;
	}
	
	private void Snap() {
		Vector3 pos = transform.position;
		pos.x = Adjust(pos.x);
		pos.y = Adjust(pos.y);
		transform.position = pos;
	}
	
	void OnMouseUp() {
		if (wasDragged) {
			wasDragged = false;
			Snap();
		}
	}
}