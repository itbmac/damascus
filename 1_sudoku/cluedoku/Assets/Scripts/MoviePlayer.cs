using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour {

	public MovieTexture movTexture;

	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = movTexture;
		movTexture.Play();
		Debug.Log (movTexture.duration);
		StartCoroutine(AdvanceToNextLevelAfter(movTexture.duration));
	}
	
	IEnumerator AdvanceToNextLevelAfter(float seconds) {
		yield return new WaitForSeconds(seconds);
		Application.LoadLevel(Application.loadedLevel + 1);
	}
}
