using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Submit : MonoBehaviour {

	public Sprite spriteDisabled;
	public Sprite spriteEnabled;
	public Sprite spriteHighlighted;
	public AudioClip ClickedWhenDisabled;
	public GameObject PopupValid;
	public GameObject PopupInvalid;
	private SpriteRenderer spriteRenderer;

	private bool submitEnabled;
	private bool currentlyHovering = false;

	void Start(){
		spriteRenderer = (SpriteRenderer)renderer;
		
		PopupInvalid.GetComponent<Popup>().Dismissed += delegate() {
			 BoardManager.Instance.ShakeInvalidTiles();
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
		if (GameManager.Instance.CurrentPopup != null)
			return;
	
		if (submitEnabled)
			SubmitBoard();
		else if (ClickedWhenDisabled) {
			audio.PlayOneShot(ClickedWhenDisabled);
		}
	}
	
	void SubmitBoard() {	
		BoardState bs = BoardManager.Instance.GetCurrentBoardState();
		if (bs == BoardState.Valid) {
			Debug.Log ("Valid board.");
			PopupValid.SendMessage("Trigger");			
		} else {
			Debug.Log ("Submitted. Invalid board. Reason: " + bs.ToString ());
			PopupInvalid.
			SendMessage("Trigger");
		}
	}

	void OnMouseEnter() {
		currentlyHovering = true;
	}
	
	void OnMouseExit() {
		currentlyHovering = false;
	}
}
