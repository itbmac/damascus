using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
public class Egg : MonoBehaviour, Colorable {
	
	bool isReal;
	bool splat;
	Animator anim;
	
	void Start() {
		anim = GetComponent<Animator>();
	}
	
	public void OnTurnReal() {
		print ("Turn real");
		isReal = true;
		anim.SetTrigger ("Real Idle");
		rigidbody2D.isKinematic = false || splat;
	}
	
	public void OnTurnDrawing() {
		print ("Turn drawing");
		isReal = false;
		rigidbody2D.isKinematic = true;		
		
		if (splat)
			Destroy(gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (isReal && coll.gameObject.layer == LayerMask.NameToLayer("Default")) {
			splat = true;
			anim.SetTrigger("Splat");
			rigidbody2D.isKinematic = true;
			GetComponent<ColorToggle>().RealColor = new Color(1, 1, 0);
		}		
	}
}
