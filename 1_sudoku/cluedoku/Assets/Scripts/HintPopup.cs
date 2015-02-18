using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class HintPopup : MonoBehaviour {

	private ClickForHint clickForHint;
	public GameObject HintAllTilesValidPopup;

	// Use this for initialization
	void Start () {
		clickForHint = FindObjectOfType<ClickForHint>();		
		gameObject.SetActive(false);		
	}
	
	public void Trigger() {
		GameManager.Instance.CurrentPopup = true;
		gameObject.SetActive(true);	
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
	
	void Dismiss() {
		GameManager.Instance.CurrentPopup = false;		
		gameObject.SetActive(false);
	}
}
