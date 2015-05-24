using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour {

	public MovieTexture movTexture;
	public float numSecondsToEndEarly = 2.0f;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.mainTexture = movTexture;
		movTexture.Play();
		GetComponent<AudioSource>().Play();
		Debug.Log (movTexture.duration);
		Cursor.visible = false;
		StartCoroutine(AdvanceToNextLevelAfter(movTexture.duration));
	}
	
	bool isDone = false;
	
	void Update() {
		if (!isDone)
			Cursor.visible = false;
	}
	
	IEnumerator AdvanceToNextLevelAfter(float seconds) {
		yield return new WaitForSeconds(seconds - numSecondsToEndEarly);
		isDone = true;
		Cursor.visible = true;
		Application.LoadLevel(Application.loadedLevel + 1);
	}
}
