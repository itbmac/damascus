using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[System.NonSerialized]
	public bool CurrentPopup = false;
	
	private int clickBlockSemaphore = 0;
	
	public bool ClickBlocked {
		get {
			return clickBlockSemaphore > 0 || CurrentPopup;
		}
	}
	
	public static GameManager Instance;
	
	public AudioClip PinDrop;

	void Awake () {
		Instance = this;
	}
	
	void Start() {
		Debug.LogWarning("Missing pin drop sound!");
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
