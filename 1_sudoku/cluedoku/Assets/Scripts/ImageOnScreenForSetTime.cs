using UnityEngine;
using System.Collections;

public class ImageOnScreenForSetTime : MonoBehaviour {
	public float OnScreenPeriod = 1.5f;
	public GameObject EnableOnceComplete;

	// Use this for initialization
	void Start () {
		EnableOnceComplete.SetActive(false);
		StartCoroutine(WaitForPeriod());
	}
	
	IEnumerator WaitForPeriod() {
		yield return new WaitForSeconds(OnScreenPeriod);
		EnableOnceComplete.SetActive(true);
		gameObject.SetActive(false);
	}
}
