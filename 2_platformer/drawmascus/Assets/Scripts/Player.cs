using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

	[System.NonSerialized]
	public float HorizontalSpeed = 20.0f;
	public float JumpSpeed = 24.0f;
	public float NormalGravity = 12.0f;
	public float JumpGravity = 4.0f;

	public Color currentColor = Color.white;
	
	private void UpdateLocalScale() {
		Vector3 newLocalScale = transform.localScale;
		if (FacingRight)
			newLocalScale.x = Mathf.Abs (newLocalScale.x);
		else
			newLocalScale.x = -Mathf.Abs (newLocalScale.x);
		transform.localScale = newLocalScale;
	}
	
	private bool _facingRight; // do not change directly outside of FacingRight property
	private bool FacingRight {
		set {
			if (value == _facingRight) return;
			_facingRight = value;
			UpdateLocalScale();
		}
		
		get {
			return _facingRight;
		}
	}

	private void colorToggle(){
		print("colorToggle called.");

		//If the wolf is white, then it is trying to take a color.
		if(currentColor == Color.white){
		}

		//Otherwise, the wolf is trying to give a color.
		else{
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	void Update() {
		//This keeps breaking on uneven platforms.....

		/*
		Vector2 pos = transform.position;
		Vector2 groundPos = pos - new Vector2(0, collider2D.bounds.extents.y + .1f);
		Vector2 velocity = rigidbody2D.velocity;
		
		int layerMask = LayerMask.GetMask("Level");

		// TODO: convert to box check
		bool grounded = Physics2D.Linecast(pos, groundPos, layerMask);

		if (grounded) {			
			if (Input.GetAxisRaw("Vertical") > 0)
				// jump triggered
				velocity.y = JumpSpeed;
			rigidbody2D.gravityScale = JumpGravity;
		} else {
			if (Input.GetAxisRaw("Vertical") == 0) {
				rigidbody2D.gravityScale = NormalGravity;
			}
		}

		// Don't change direction on 0 or avatar will awkwardly face
		// one way if no key is pressed
		if (Input.GetAxis("Horizontal") > 0)
			FacingRight = true;
		else if (Input.GetAxis("Horizontal") < 0)
			FacingRight = false;
		
		velocity.x = Input.GetAxis("Horizontal") * HorizontalSpeed;
		rigidbody2D.velocity = velocity;
		*/

		Vector2 velocity = rigidbody2D.velocity;
		Vector3 normal = transform.up;

		//Update up/down movement.

		//Update right/left movement.
		if (Input.GetAxis("Horizontal") > 0)
			FacingRight = true;
		else if (Input.GetAxis("Horizontal") < 0)
			FacingRight = false;
		
		velocity.x = Input.GetAxis("Horizontal") * HorizontalSpeed;
		rigidbody2D.velocity = velocity;

		//Fix orientation so wolf doesn't end up upside down.
		if(normal.y < 0.9f){
			normal.y = 0.9f;
			transform.up = normal;
		}
	}
	
	Vector2 GetForward() {
		if (FacingRight)
			return -transform.right;
		else
			return transform.right;
	}
}
