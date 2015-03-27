using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
public class Egg : Colorable {
	
	bool isReal;
	bool splat;
	Animator anim;
	public GameObject Drop;
	
	void Start() {
		anim = GetComponent<Animator>();
	}
	
	public override void OnTurnReal() {
		print ("Turn real");
		isReal = true;
		anim.SetTrigger ("Real Idle");
		rigidbody2D.isKinematic = false || splat;
	}
	
	public override void OnTurnDrawing() {
		print ("Turn drawing");
		isReal = false;
		rigidbody2D.isKinematic = true;		
		
		if (splat && Drop != null) {
			Instantiate(Drop, transform.position, Quaternion.identity);		
			Destroy(gameObject);
		}
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
