using UnityEngine;
using System.Collections;

public class FileViewer : MonoBehaviour {

	public static FileViewer Instance;
	
	public AudioClip PickUpFile;
	public AudioClip PutDownFile;
	
	float lastShown = -10;
	const float TIME_THRESHOLD = 0.1f;
	
//	public Vector3 AlternatePosition;
//	private Vector3 DefaultPosition;
	
	public Sprite CurrentSprite {
		get {
			return ((SpriteRenderer)renderer).sprite;
		}
	}

	// Use this for initialization
	void Awake () {
		Instance = this;
	}
	
	void Start() {
		Hide(true);
//		DefaultPosition = transform.position;
	}
	
	public void Show(Sprite sprite) {
//		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
//		if (Vector3.Distance(DefaultPosition, curScreenPoint) > Vector3.Distance(AlternatePosition, curScreenPoint)) {
//			transform.position = DefaultPosition;
//		} else {
//			transform.position = AlternatePosition;
//		}
	
		lastShown = Time.time;
		var sr = (SpriteRenderer)renderer;
		sr.sprite = sprite;
		renderer.enabled = true;
		audio.PlayOneShot(PickUpFile);
	}
	
	void Hide(bool silent = false) {
		if (Time.time - lastShown < TIME_THRESHOLD)
			return;
			
		renderer.enabled = false;
		if (!silent)
			audio.PlayOneShot(PutDownFile);
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer.enabled && Input.GetMouseButtonDown(0)) {
			Hide ();
		}
	}
}
