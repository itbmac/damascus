using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Submit : MonoBehaviour {

	public Sprite spriteDisabled;
	public Sprite spriteEnabled;
	public AudioClip ClickedWhenDisabled;
	public GameObject PopupValid;
	public GameObject PopupInvalid;
	
	private bool submitEnabled;
	
	void Update() {
		SpriteRenderer spriteRenderer = (SpriteRenderer)renderer;
		
		submitEnabled = BoardManager.Instance.IsCurrentBoardFilled();
		if (submitEnabled)
			spriteRenderer.sprite = spriteEnabled;
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
			PopupInvalid.SendMessage("Trigger");
		}
	}
}
