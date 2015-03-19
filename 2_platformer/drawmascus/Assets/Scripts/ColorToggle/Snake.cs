using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
[RequireComponent(typeof(Rigidbody2D))]
public class Snake : MonoBehaviour, Colorable {

	public GameObject Target;
	
	public float Speed = 5.0f;
	
	private bool isReal;
	
	public void OnTurnReal() {
		// Chase the bird
		
		isReal = true;
		rigidbody2D.isKinematic = false;
	}
	
	public void OnTurnDrawing() {
		// Stop chasing the bird
		
		isReal = false;
		rigidbody2D.isKinematic = true;
	}
	
	void Update() {
		if (isReal && Target != null) {
			Vector2 velocity = rigidbody2D.velocity;
			velocity.x = Mathf.Sign(Target.transform.position.x - transform.position.x) * Speed;
			rigidbody2D.velocity = velocity;			
		}		
	}
}
