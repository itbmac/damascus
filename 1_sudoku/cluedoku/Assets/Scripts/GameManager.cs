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
	
	void Update() {
		if (Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.L) && Input.anyKeyDown)
			Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
			
		if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.L) && Input.anyKeyDown)
			Application.LoadLevel(Application.loadedLevel == 0 ? Application.levelCount - 1 : (Application.loadedLevel - 1) % Application.levelCount);
			
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
			
		// GM = go to main, the last level
		if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.M) && Input.anyKeyDown)
			Application.LoadLevel(Application.levelCount - 1);
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
