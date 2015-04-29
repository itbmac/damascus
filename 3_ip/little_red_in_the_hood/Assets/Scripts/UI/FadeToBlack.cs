using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour {

	public float TimeToFade = 1.0f;
	public float PauseTime = 0.5f;
	
	Image image;

	IEnumerator GameOverCoroutine()
	{
		image.enabled = true;
		TheGameManager.Instance.GameOver = true;
		
		yield return new WaitForSeconds(PauseTime);
			
		float endTime = Time.time + TimeToFade;
		
		var color = image.color;	
		while (Time.time <= endTime) {			
			color.a = 1 - ((endTime - Time.time) / TimeToFade);
			image.color = color;
			
			yield return null;
		}		
		
		Application.LoadLevel(Application.loadedLevel);
	}

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		image.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Trigger() {
		// do not call more than once in a level!
		StartCoroutine(GameOverCoroutine());
	}
}
