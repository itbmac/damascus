using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	public bool PeaceAndQuietForDevelopers = true;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		if (PeaceAndQuietForDevelopers)
			GetComponent<AudioSource>().enabled = false;
#endif
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
