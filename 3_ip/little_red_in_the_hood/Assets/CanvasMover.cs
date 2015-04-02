using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CanvasMover : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable() {
		Vector3 pos = transform.position;
		if(Application.isEditor&&!Application.isPlaying) //only run it if we are in edit mode.
			pos.y =585;
		else{
			pos.y =0;    
		}
		transform.position = pos;
	}
}
