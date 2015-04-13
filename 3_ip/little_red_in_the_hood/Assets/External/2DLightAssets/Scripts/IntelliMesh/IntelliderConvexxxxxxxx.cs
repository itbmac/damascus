using UnityEngine;
using System.Collections;

//-- Martin Ysa Copyright 2015-- //
//-- ONLY WORKS IN SINGLE LIGHT --//

[ExecuteInEditMode]

public class IntelliderCovenx : MonoBehaviour {

	public GameObject LightTarget;
	GameObject mCollider;

	Vector2 max;
	Vector2 min;




	void Start () {
		LightTarget = GameObject.Find("2DLight");
		mCollider = gameObject.transform.FindChild("collider").gameObject;
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

			

		if(!mCollider){
			mCollider = gameObject.transform.FindChild("collider").gameObject;
			if(!mCollider){
				Debug.LogWarning("NOT Collider Found");
				return;
			}
		}
			


		// Calcular angulo entre posicion collider y la luz
		Vector3 lightPos = LightTarget.transform.position;
		Vector3 colliderPos = gameObject.transform.position;
		mCollider.transform.up = (lightPos - colliderPos);

	}


}
