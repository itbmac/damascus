using UnityEngine;
using System.Collections;

public class OverlayVerifyTileClick : MonoBehaviour {

	public GameObject FileViewerObj; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (FileViewerObj.GetComponent<Renderer>().enabled)
			gameObject.SetActive(false);
	}
}
