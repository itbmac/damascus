using UnityEngine;
using System.Collections;

public class ClickToDisable : MonoBehaviour {

	public GameObject followingItem, additionalObjDisableOnClick;
	public AudioClip AdvanceNoise;
	public bool disableRendererOnClick = true, playSoundOnVisible = false, mustClickOnObject = false;
	private bool prevRendererState = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer.enabled && !mustClickOnObject && Input.GetMouseButtonDown(0)) {
			ClickedOn();
		}
		else if (followingItem && renderer.enabled) {
			followingItem.SetActive(false);
		}

		if (!prevRendererState && renderer.enabled && AdvanceNoise) {
			audio.PlayOneShot(AdvanceNoise);
		}

		prevRendererState = renderer.enabled;
	}

	void OnMouseDown() {
		if (renderer.enabled && mustClickOnObject)
			ClickedOn();
	}

	void ClickedOn() {
		if (prevRendererState) {
			audio.PlayOneShot(AdvanceNoise);
			if (followingItem) {
				followingItem.SetActive(true);
				if (followingItem.renderer)
					followingItem.renderer.enabled = true;
			}
			
			if (disableRendererOnClick)
				renderer.enabled = false;
			
			if (additionalObjDisableOnClick)
				additionalObjDisableOnClick.SetActive(false);
		}
	}
}
