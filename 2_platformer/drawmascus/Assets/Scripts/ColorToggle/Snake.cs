using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
[RequireComponent(typeof(Rigidbody2D))]
public class Snake : Colorable {

	public GameObject Target;
	
	public float Speed = 5.0f;
	
	private bool isReal;
	
	Animator anim;

	private bool isActing = false;
	
	public AudioClip Sound;
	
	void Start() {
		anim = GetComponent<Animator>();
	}
	
	public override void OnTurnReal() {
		// Chase the bird
		
		isReal = true;
		rigidbody2D.isKinematic = false;
		anim.SetBool("Slither", true);
	}
	
	public override void OnTurnDrawing() {
		// Stop chasing the bird
		
		isReal = false;
		rigidbody2D.isKinematic = true;
		anim.SetBool("Slither", false);
	}
	
	void Update() {
		if (isReal) {
			if (Target == null) {
				Destroy(gameObject);
			} else {
				if (!isActing){
					audio.PlayOneShot(Sound);
					isActing = true;
				}
				Vector2 velocity = rigidbody2D.velocity;
				velocity.x = Mathf.Sign(Target.transform.position.x - transform.position.x) * Speed;
				rigidbody2D.velocity = velocity;			
			}
		}		
	}
}
