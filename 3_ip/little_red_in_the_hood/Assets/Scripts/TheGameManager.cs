using UnityEngine;
using System.Collections;

public class TheGameManager : MonoBehaviour {

	public float DetectedTimeRemaining {
		get {
			return Mathf.Max(0, DetectedStartTime + DetectionTime - Time.time);
		}
	}
	
	public float DetectionTime = 2.0f;	
	private float DetectedStartTime = -3f;
	public bool DetectedCurrently = false;
	
	public static TheGameManager Instance { get; private set; }
	
	void Awake() 
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (DetectedCurrently && (DetectedTimeRemaining <= 0)) {
			Camera.main.GetComponent<CamFollow>().ChangeSize(Camera.main.GetComponent<CamFollow>().cameraSizeStart);
			DetectedCurrently = false;
		}
		
		// NL = next level
		if (Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.L) && Input.anyKeyDown) {
			Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
		}
		
		// PL = previous level
		if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.L) && Input.anyKeyDown) {
			Application.LoadLevel(Application.loadedLevel == 0 ? Application.levelCount - 1 : Application.loadedLevel - 1);
		}
	}
	
	public void Detected() {
		DetectedStartTime = Time.time;
		Camera.main.GetComponent<CamFollow>().ChangeSize(Camera.main.GetComponent<CamFollow>().cameraSizeZoomed);

		if (!DetectedCurrently)
			Camera.main.GetComponent<CamFollow>().Shake(Camera.main.GetComponent<CamFollow>().SHAKE_INTENSITY_4);
		DetectedCurrently = true;
	}
}
