using UnityEngine;
using System.Collections;

public class ClickToDisable : MonoBehaviour {

	public GameObject followingItem;
	public AudioClip AdvanceNoise;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer.enabled && Input.GetMouseButtonDown(0)) {
			audio.PlayOneShot(AdvanceNoise);
			if (followingItem)
				followingItem.SetActive(true);
			renderer.enabled = false;
		}
	}
}
