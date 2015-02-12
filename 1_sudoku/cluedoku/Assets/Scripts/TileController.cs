using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

// from https://stackoverflow.com/questions/23152525/drag-object-in-unity-2d
public class TileController : MonoBehaviour {
	public AudioClip noisePickUp, noiseDropOff;
	public Sprite infoCard;
	private Vector3 screenPoint;
	private Vector3 offset;
	public Vector2 offsetPixel = new Vector2(.45f, -1.2f);
	const float SIZE = 2.95f;
	const float CLICK_DISTANCE_THRESHOLD = 0.1f;
	Vector3 mouseDownStartPos;
	
	void Start() {
		Snap ();
		
		if (tag != "Tile")
			Debug.LogError("Must have tag Tile");
	}
	
	void OnMouseDown() {
		mouseDownStartPos = transform.position;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		audio.PlayOneShot(noisePickUp);
	}
	
	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	}
	
	private float Adjust(float f) {
		return (Mathf.Round(.5f + f / SIZE) - .5f) * SIZE;
	}
	
	private void Snap() {
		Vector3 pos = transform.position;
		pos.x = Adjust(pos.x) + offsetPixel.x;
		pos.y = Adjust(pos.y) + offsetPixel.y;
		transform.position = pos;
	}
	
	void OnMouseUp() {
		Snap();
		if (infoCard && Vector3.Distance(transform.position, mouseDownStartPos) < CLICK_DISTANCE_THRESHOLD) {
			FileViewer.Instance.Show(infoCard);
		} else {
			audio.PlayOneShot(noiseDropOff);
		}
	}
}