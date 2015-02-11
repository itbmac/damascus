using UnityEngine;
using System.Collections;

public class ClickToDisable : MonoBehaviour {

	public GameObject followingItem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (renderer.enabled && Input.GetMouseButtonDown(0)) {
			if (followingItem)
				followingItem.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
