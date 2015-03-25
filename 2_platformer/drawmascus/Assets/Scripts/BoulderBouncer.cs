using UnityEngine;
using System.Collections;

public class BoulderBouncer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Boulder") {
			print ("Apply teh force");
			Vector2 f = coll.transform.position - transform.position;
			f.y = 0;
			f.Normalize();
			f.y = -1;
			
			coll.rigidbody.AddForce(f * 5000.0f);
		}		
	}
}
