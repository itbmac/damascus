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
		if (GetComponent<Renderer>().enabled && !mustClickOnObject && Input.GetMouseButtonDown(0)) {
			ClickedOn();
		}
		else if (followingItem && GetComponent<Renderer>().enabled) {
			followingItem.SetActive(false);
		}

		if (!prevRendererState && GetComponent<Renderer>().enabled && OnVisibleNoise) {
			GetComponent<AudioSource>().PlayOneShot(OnVisibleNoise);

			if (enableAllChildren) {
				foreach (Transform child in transform) {
					child.gameObject.SetActive(true);
				}
			}
		}

		prevRendererState = GetComponent<Renderer>().enabled;
	}

	void OnMouseDown() {
		if (GetComponent<Renderer>().enabled && mustClickOnObject)
			ClickedOn();
	}
	
	void Run() {
		GetComponent<AudioSource>().PlayOneShot(AdvanceNoise);
		if (followingItem) {
			followingItem.SetActive(true);
			if (followingItem.GetComponent<Renderer>())
				followingItem.GetComponent<Renderer>().enabled = true;
		}
		
		if (disableRendererOnClick) {
			GetComponent<Renderer>().enabled = false;

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
