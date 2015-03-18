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
	}
	
	public void OnTurnDrawing() {
		// Stop chasing the bird
		
		isReal = false;
	}
	
	void Update() {
		if (isReal) {
			Vector2 velocity = rigidbody2D.velocity;
			velocity.x = Mathf.Sign(Target.transform.position.x - transform.position.x) * Speed;
			rigidbody2D.velocity = velocity;			
		}		
	}
}
