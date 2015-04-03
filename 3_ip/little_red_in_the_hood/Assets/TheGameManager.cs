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
	
	}
	
	public void Detected() {
		DetectedStartTime = Time.time;
	}
}
