using UnityEngine;
using System.Collections;

public class interface_touch: MonoBehaviour {
	
	GameObject cLight;
	GameObject cubeL;
	
	//GUIText UIlights;
	//GUIText UIvertex;


	[HideInInspector] public static int vertexCount;


	
	// Update is called once per frame
	void Update () {
		cLight = GameObject.Find("2DLight");
		//if(Input.GetAxis("Horizontal")){
		//light.transform.position = new Vector3 (Input.mousePosition.x -Screen.width*.5f, Input.mousePosition.y -Screen.height*.5f);
		Vector3 pos = cLight.transform.position;
		pos.x += Input.GetAxis ("Horizontal") * 3f * Time.deltaTime;
		pos.y += Input.GetAxis ("Vertical") * 3f * Time.deltaTime;
		cLight.transform.position = pos;

	}



}
