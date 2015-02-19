using UnityEngine;
using System.Collections;

public class HintPopup : MonoBehaviour {

	private ClickForHint clickForHint;
	public GameObject HintAllTilesValidPopup;
	private bool open;

	// Use this for initialization
	void Start () {
		clickForHint = FindObjectOfType<ClickForHint>();		
		gameObject.SetActive(false);
		open = false	;	
	}
	
	public void Trigger() {
		GameManager.Instance.CurrentPopup = true;
		gameObject.SetActive(true);	
		((SpriteRenderer)renderer).enabled = true;
		open = true;
	}
	
	public void PopupButtonClicked(string buttonName) {
		if (buttonName == "PlaceValidTile") {
			bool tilePlaced = BoardManager.Instance.PlaceValidTile();
			if (!tilePlaced)
				HintAllTilesValidPopup.SendMessage("Trigger");
		
		} else if (buttonName == "ShakeInvalidTiles") {
			int numInvalidTiles = BoardManager.Instance.ShakeInvalidTiles();
			if (numInvalidTiles == 0) {
				HintAllTilesValidPopup.SendMessage("Trigger");
			}
		}
		
		clickForHint.RemainingNumHints -= 1;
		
		Dismiss();
	}
	
	void OnMouseDown() {
		if (!open)
			return;
		
		Dismiss();
	}
	
	void Dismiss() {
		GameManager.Instance.CurrentPopup = false;	
		((SpriteRenderer)renderer).enabled = false;	
		gameObject.SetActive(false);
		open = false;
	}
}
