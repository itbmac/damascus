using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
public class Egg : MonoBehaviour, Colorable {

	public GameObject Yolk;
	
	bool isReal;
	bool splat;
	Animator anim;
	
	void Start() {
		anim = GetComponent<Animator>();
		anim.SetTrigger ("Drawing Idle");
	}
	
	public void OnTurnReal() {
		isReal = true;
		anim.SetTrigger ("Idle");
		rigidbody2D.isKinematic = false || splat;
	}
	
	public void OnTurnDrawing() {
		isReal = false;
		anim.SetTrigger ("Yolk Idle");
		rigidbody2D.isKinematic = true;		
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
