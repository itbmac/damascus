using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
public class Egg : MonoBehaviour, Colorable {

	public GameObject Yolk;
	
	bool isReal;
	
	public void OnTurnReal() {
		isReal = true;
		rigidbody2D.isKinematic = false;
	}
	
	public void OnTurnDrawing() {
		isReal = false;
		rigidbody2D.isKinematic = true;
		
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (isReal && coll.gameObject.layer == LayerMask.NameToLayer("Default")) {
			Instantiate(Yolk, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}		
	}
}
