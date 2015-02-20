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
		if (GameManager.Instance.tutorialState == GameManager.TutorialState.MustCheckConsistency &&
		    buttonName != "ShakeInvalidTiles")
		    return;
		if (GameManager.Instance.tutorialState == GameManager.TutorialState.MustHaveEpiphany &&
		    buttonName != "PlaceValidTile")
			return;	
	
		if (buttonName == "PlaceValidTile") {
			if (GameManager.Instance.tutorialState == GameManager.TutorialState.MustHaveEpiphany)
				GameManager.Instance.tutorialState = GameManager.TutorialState.None;
				
			bool tilePlaced = BoardManager.Instance.PlaceValidTile();
			if (!tilePlaced)
				HintAllTilesValidPopup.SendMessage("Trigger");
		
		} else if (buttonName == "ShakeInvalidTiles") {
			if (GameManager.Instance.tutorialState == GameManager.TutorialState.MustCheckConsistency)
				GameManager.Instance.tutorialState = GameManager.TutorialState.MustHaveEpiphany;
		
			int numInvalidTiles = BoardManager.Instance.ShakeInvalidTiles();
			if (numInvalidTiles == 0) {
				HintAllTilesValidPopup.SendMessage("Trigger");
			}
		}
		
		clickForHint.UseAHint();
		
		Dismiss();
	}
	
	void OnMouseDown() {
		if (!open)
			return;
			
		if (GameManager.Instance.tutorialState != GameManager.TutorialState.None)
			return;

		clickForHint.CancelHintUse();

		Dismiss();
	}
	
	void Dismiss() {
		GameManager.Instance.CurrentPopup = false;	
		((SpriteRenderer)renderer).enabled = false;	
		gameObject.SetActive(false);
		open = false;
	}
}
