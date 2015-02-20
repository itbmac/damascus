using UnityEngine;
using System.Collections;

public class ClickToDisable : MonoBehaviour {

	public GameObject followingItem, additionalObjDisableOnClick;
	public AudioClip OnVisibleNoise, AdvanceNoise;
	public bool disableRendererOnClick = true, playSoundOnVisible = false, mustClickOnObject = false, runOnStartIfNotEditor = false, enableAllChildren = false;
	private bool prevRendererState = false;
	public bool OverrideClickBlock = false;

	// Use this for initialization
	void Start () {
#if !UNITY_EDITOR
		if (runOnStartIfNotEditor)
			Run();
#endif
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer.enabled && !mustClickOnObject && Input.GetMouseButtonDown(0)) {
			ClickedOn();
		}
		else if (followingItem && renderer.enabled) {
			followingItem.SetActive(false);
		}

		if (!prevRendererState && renderer.enabled && OnVisibleNoise) {
			audio.PlayOneShot(OnVisibleNoise);

			if (enableAllChildren) {
				foreach (Transform child in transform) {
					child.gameObject.SetActive(true);
				}
			}
		}

		prevRendererState = renderer.enabled;
	}

	void OnMouseDown() {
		if (renderer.enabled && mustClickOnObject)
			ClickedOn();
	}
	
	void Run() {
		audio.PlayOneShot(AdvanceNoise);
		if (followingItem) {
			followingItem.SetActive(true);
			if (followingItem.renderer)
				followingItem.renderer.enabled = true;
		}
		
		if (disableRendererOnClick) {
			renderer.enabled = false;

			if (enableAllChildren) {
				foreach (Transform child in transform) {
					child.gameObject.SetActive(false);
				}
			}
		}
		
		if (additionalObjDisableOnClick)
			additionalObjDisableOnClick.SetActive(false);
	}

	void ClickedOn() {
		if (GameManager.Instance.ClickBlocked && !OverrideClickBlock)
			return;
	
		if (prevRendererState) {
			Run();
		}
	}
}
