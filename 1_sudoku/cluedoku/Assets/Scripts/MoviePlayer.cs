using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour {

	public MovieTexture movTexture;

	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = movTexture;
		movTexture.Play();
		audio.Play();
		Debug.Log (movTexture.duration);
		StartCoroutine(AdvanceToNextLevelAfter(movTexture.duration));
	}
	
	IEnumerator AdvanceToNextLevelAfter(float seconds) {
		yield return new WaitForSeconds(seconds-1);
		Application.LoadLevel(Application.loadedLevel + 1);
	}
}
