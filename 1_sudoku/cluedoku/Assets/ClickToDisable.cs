using UnityEngine;
using System.Collections;

public class ClickToDisable : MonoBehaviour {

	public GameObject followingItem, additionalObjDisableOnClick;
	public AudioClip AdvanceNoise;
	public bool disableRendererOnClick = true, playSoundOnVisible = false;
	private bool prevRendererState = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer.enabled && Input.GetMouseButtonDown(0)) {
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
		else if (renderer.enabled) {
			followingItem.SetActive(false);
		}

		if (!prevRendererState && renderer.enabled) {
			audio.PlayOneShot(AdvanceNoise);
		}

		prevRendererState = renderer.enabled;
	}
}
