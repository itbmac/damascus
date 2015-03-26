using UnityEngine;
using System.Collections;

public class L2EndRock : MonoBehaviour {

	bool knocked = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		if (!knocked && col.collider.name == "Boulder") {
			rigidbody2D.isKinematic = false;
			knocked = true;
			rigidbody2D.AddForceAtPosition(new Vector2(1000, 0), (Vector2)transform.position + new Vector2(0, collider2D.bounds.extents.y * 0.5f));
		}
		
		
	}
}
