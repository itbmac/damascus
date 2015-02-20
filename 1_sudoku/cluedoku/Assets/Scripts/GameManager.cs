using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[System.NonSerialized]
	public bool CurrentPopup = false;
	
	public bool BlockFileViewer = false;
	
	public enum TutorialState {None, MustCheckConsistency, MustHaveEpiphany}
	public TutorialState tutorialState = TutorialState.None;
	
	private int clickBlockSemaphore = 0;
	
	public bool ClickBlocked {
		get {
			return clickBlockSemaphore > 0 || CurrentPopup || tutorialState != TutorialState.None;
		}
	}
	
	public static GameManager Instance;
	
	public AudioClip PinDrop;

	void Awake () {
		Instance = this;
	}
	
	void Start() {

	}
	
	public void AddClickBlock() {
		clickBlockSemaphore += 1;
	}
	
	public void RemoveClickBlock() {
		clickBlockSemaphore -= 1;
		if (clickBlockSemaphore < 0)
			Debug.LogError("clickBlockSemaphore < 0");
	}
}
