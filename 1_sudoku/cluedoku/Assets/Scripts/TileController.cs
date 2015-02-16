using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

// from https://stackoverflow.com/questions/23152525/drag-object-in-unity-2d
public class TileController : MonoBehaviour {
	public AudioClip noisePickUp, noiseDropOff;
	public Sprite infoCard;
	private Vector3 screenPoint;
	private Vector3 offset;
	private bool pickedUp;
	
	const float CLICK_DISTANCE_THRESHOLD = 0.1f;
	Vector3 mouseDownStartPos;
	private bool locked = false;
	public GameObject pin;
	const float pinRandomness = -0.2f;
	private Vector3 pinPositionOriginal;
	
	void Start() {		
		if (tag != "Tile")
			Debug.LogError("Must have tag Tile");

		pinPositionOriginal = pin.transform.localPosition;

		Reset();
	}
	
	public void Reset() {
		Snap ();
		locked = BoardManager.Instance.IsOnBoard(transform.position);	

		if (locked) {
			pin.renderer.enabled = true;
			pin.transform.localPosition = new Vector3(pinPositionOriginal.x + Random.Range(-2 * pinRandomness, -1 * pinRandomness),
			                                     	  pinPositionOriginal.y + Random.Range(-2 * pinRandomness, -1 * pinRandomness),
			                                          pinPositionOriginal.z);
		}
		else pin.renderer.enabled = false;
	}
	
	void OnMouseDown() {
		if (GameManager.Instance.CurrentPopup != null)
			return;
			
		if (locked) {
			if (infoCard)
				FileViewer.Instance.Show(infoCard);
			return;
		}
	
		pickedUp = true;
		mouseDownStartPos = transform.position;
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		audio.PlayOneShot(noisePickUp);
		((SpriteRenderer)renderer).sortingLayerName = "ActiveTile";
	}
	
	void OnMouseDrag() {
		if (!pickedUp)
			return;
			
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = curPosition;
	}
	
	private void Snap() {
		transform.position = BoardManager.Instance.SnapPos(transform.position);
	}	
	
	private void SnapToNewPosIfOpen() {
		if (BoardManager.Instance.IsPositionOpen(transform.position, gameObject))
			transform.position = BoardManager.Instance.SnapPosConstrained(transform.position);
		else
			transform.position = mouseDownStartPos;
	}
	
	void OnMouseUp() {
		if (!pickedUp)
			return;
		pickedUp = false;
			
		if (infoCard && Vector3.Distance(transform.position, mouseDownStartPos) < CLICK_DISTANCE_THRESHOLD) {
			FileViewer.Instance.Show(infoCard);
		} else {
			audio.PlayOneShot(noiseDropOff);
		}
		
		SnapToNewPosIfOpen();
		((SpriteRenderer)renderer).sortingLayerName = "Default";
	}
}