using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Submit : MonoBehaviour {

	public bool pinCorrectlyPlacedTilesOnSubmit = false;
	public Sprite spriteDisabled;
	public Sprite spriteEnabled;
	public Sprite spriteHighlighted;
	public AudioClip ClickedWhenDisabled;
	public GameObject PopupValid;
	public GameObject PopupInvalid;
	private SpriteRenderer spriteRenderer;
	
	public Sprite InvalidCounts;
	public Sprite InvalidPair;

	private bool submitEnabled;
	private bool currentlyHovering = false;

	void Start(){
		if (InvalidCounts == null)
			Debug.LogWarning("Missing InvalidCounts");
	
		if (InvalidPair == null)
			Debug.LogWarning("Missing InvalidPair");
	
		spriteRenderer = (SpriteRenderer)GetComponent<Renderer>();
		
		PopupInvalid.GetComponent<Popup>().Dismissed += delegate() {
			BoardManager.Instance.ShakeInvalidTiles(pinCorrectlyPlacedTilesOnSubmit);
		};
	}

	void Update() {
		submitEnabled = BoardManager.Instance.IsCurrentBoardFilled();
		if (submitEnabled) {
			if(currentlyHovering) 
				spriteRenderer.sprite = spriteHighlighted;
			else 
				spriteRenderer.sprite = spriteEnabled;
		}
		else
			spriteRenderer.sprite = spriteDisabled;
	}

	void OnMouseDown() {
		if (GameManager.Instance.ClickBlocked)
			return;
	
		if (submitEnabled)
			SubmitBoard();
		else if (ClickedWhenDisabled) {
			GetComponent<AudioSource>().PlayOneShot(ClickedWhenDisabled);
		}
	}
	
	void SubmitBoard() {	
		BoardState bs = BoardManager.Instance.GetCurrentBoardState();
		if (bs == BoardState.Valid) {
			Debug.Log ("Valid board.");
			PopupValid.SendMessage("Trigger");			
		} else {
			var PopupInvalidScript = PopupInvalid.GetComponent<Popup>();
			Debug.Log ("Submitted. Invalid board. Reason: " + bs.ToString ());
			
			if (bs == BoardState.InvalidCounts) {
				PopupInvalidScript.MySprite = InvalidCounts;
			} else if (bs == BoardState.InvalidPair) {
				PopupInvalidScript.MySprite = InvalidPair;
			}		
			
			PopupInvalidScript.Trigger ();
		}
	}

	void OnMouseEnter() {
		currentlyHovering = true;
	}
	
	void OnMouseExit() {
		currentlyHovering = false;
	}
}
