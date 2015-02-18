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
	
	public bool Locked {
		get {
			return locked;
		}
	}
	
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
		if (GameManager.Instance.CurrentPopup)
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
	
	
	int activeSlide = 0;
	const float SLIDE_TIME = 1.0F; // not exactly time...
	private IEnumerator SlideToPos(Vector2 pos, bool resetAfter = false) {
		activeSlide += 1;
		int mySlide = activeSlide;
	
		((SpriteRenderer)renderer).sortingLayerName = "SlidingTile";
		// Note: during sliding time, normal tile rules aren't enforced >.>
		while (Vector2.Distance(transform.position, pos) > 0.1F) {
			if (activeSlide != mySlide)
				return false; // another slide is active
		
			transform.position = Vector2.Lerp(transform.position, pos, 0.4f);
			yield return new WaitForSeconds(0.025f);
		}
		((SpriteRenderer)renderer).sortingLayerName = "Default";
		
		if (resetAfter)
			Reset ();
	}	
	
	public bool RequestMove(Vector2 pos) {	
		if (!locked) {
			StartCoroutine(SlideToPos(pos));
			return true;
		}
		
		return false;
	}
	
	public void MoveAndReset(Vector2 pos) {
		StartCoroutine(SlideToPos(pos, true));
	}
	
	private void SnapToNewPosIfOpen() {
		Vector2 attemptedPosition = BoardManager.Instance.SnapPosConstrained(transform.position);
	
		if (BoardManager.Instance.IsPositionOpen(transform.position, gameObject))
			transform.position = attemptedPosition;
		else {
			var mouseDownStartCoord = mouseDownStartPos.ToGridCoord();
			var newGridCoord = transform.position.ToGridCoord();
			
			var other = BoardManager.Instance.GetTileAtPosition(newGridCoord, gameObject);
			TileController otherController = null;
			if (other)
				otherController = other.GetComponent<TileController>();
			else
				Debug.LogError("This shouldn't happen"); // because the position isn't open
			
			if (Mathf.Sign(mouseDownStartCoord.x) == Mathf.Sign(newGridCoord.x)) {
				// didn't change regions, attempt swap
				
				if (otherController.RequestMove(mouseDownStartPos)) {				
					transform.position = attemptedPosition;
				} else {
					transform.position = mouseDownStartPos;
				}				
				
			} else {
				// find nearest
				
				Vector2? closest;
				if (newGridCoord.x >= 0)
					closest = BoardManager.Instance.FindClosestOpenPositionOnBoard(transform.position, gameObject);
				else
					closest = BoardManager.Instance.FindClosestOpenPositionOnSide(transform.position, gameObject);
				
				if (closest.HasValue)
					transform.position = closest.Value;
				else {
					if (otherController.RequestMove(mouseDownStartPos)) {				
						transform.position = attemptedPosition;
					} else {
						transform.position = mouseDownStartPos;
					}
				}	
			}
		
			
		}
			
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
	
	const int SHAKE_SIZE = 20;
	const int SHAKE_SPEED = 1;
	IEnumerator ShakeCoroutine() {
//		transform.rotation.eulerAngles.z
		
		Vector3 rot = transform.eulerAngles;

		for (int i = 0; i <= SHAKE_SIZE; i += SHAKE_SPEED) {
			rot.z = i;			
			transform.eulerAngles = rot;			
			yield return new WaitForSeconds(0.01f);
		}
		for (int i = SHAKE_SIZE; i >= -SHAKE_SIZE; i -= SHAKE_SPEED) {
			rot.z = i;			
			transform.eulerAngles = rot;			
			yield return new WaitForSeconds(0.01f);
		}
		for (int i = -SHAKE_SIZE; i <= 0; i += SHAKE_SPEED) {
			rot.z = i;			
			transform.eulerAngles = rot;			
			yield return new WaitForSeconds(0.01f);
		}
		for (int i = 0; i <= SHAKE_SIZE; i += SHAKE_SPEED) {
			rot.z = i;			
			transform.eulerAngles = rot;			
			yield return new WaitForSeconds(0.01f);
		}
		for (int i = SHAKE_SIZE; i >= -SHAKE_SIZE; i -= SHAKE_SPEED) {
			rot.z = i;			
			transform.eulerAngles = rot;			
			yield return new WaitForSeconds(0.01f);
		}
		for (int i = -SHAKE_SIZE; i <= 0; i += SHAKE_SPEED) {
			rot.z = i;			
			transform.eulerAngles = rot;			
			yield return new WaitForSeconds(0.01f);
		}

		rot.z = 0;			
		transform.eulerAngles = rot;
		
		
	}
	
	public void Shake() {
		if (!locked)
			StartCoroutine(ShakeCoroutine());
	}
}