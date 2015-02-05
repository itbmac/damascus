using UnityEngine;
using System.Collections;

public class AnimateRobot : MonoBehaviour {
	SpriteRenderer render;
	int frameCount = 2;
	int currentFrame = 0;
	GUIText textbox;

	IEnumerator animate(){
		while(true){
			float timeLeft = .5f;
			//change frame
			gameObject.transform.GetChild(currentFrame).renderer.enabled = false;
			currentFrame = (currentFrame + 1) % frameCount;
			gameObject.transform.GetChild(currentFrame).renderer.enabled = true;
			while ((timeLeft -= Time.deltaTime) > 0.0f)
			{
				yield return null;
			}
		}
	}

	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<SpriteRenderer>();
		Vector3 screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		textbox = gameObject.GetComponent<GUIText>();
		textbox.pixelOffset = new Vector2(screenPos.x, screenPos.y + 30);

		StartCoroutine(animate());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
