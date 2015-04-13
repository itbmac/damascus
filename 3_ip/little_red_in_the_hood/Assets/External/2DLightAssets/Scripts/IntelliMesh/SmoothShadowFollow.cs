using UnityEngine;
using System.Collections;

//-- Martin Ysa Copyright 2015-- //
//-- ONLY WORKS ON SINGLE LIGHT --//

[ExecuteInEditMode]

public class SmoothShadowFollow : MonoBehaviour {

	public GameObject LightTarget;
	GameObject mShadow;


	void Start () {
		LightTarget = GameObject.Find("2DLight");
		mShadow = gameObject.transform.FindChild("smooth").gameObject;
	}
	
	// Update is called once per frame
	void Update () {


		if(!LightTarget){
			LightTarget = GameObject.Find("2DLight");
			if(!LightTarget){
				Debug.LogWarning("NOT 2DLight GameObject Found");
				return;
			}
		}

			

		if(!mShadow){
			mShadow = gameObject.transform.FindChild("smooth").gameObject;
			if(!mShadow){
				Debug.LogWarning("NOT Collider Found");
				return;
			}
		}
			


		// Calcular angulo entre posicion collider y la luz
		//Vector3 smoothPos = mCollider.GetComponent<PolygonCollider2D>().points[0];
		//smoothPos = transform.TransformPoint(smoothPos);

		Vector3 lightPos = LightTarget.transform.position;
		Vector3 smoothPos = gameObject.transform.position;
		mShadow.transform.up = (lightPos - smoothPos);

	}


}
