using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

	public float HorizontalSpeed = 20.0f;
	public float JumpSpeed = 24.0f;
	public float NormalGravity = 12.0f;
	public float JumpGravity = 4.0f;

	public Color CurrentColor = Color.white;
	
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
	
	public static Player Instance {
		get; private set;
	}
	
	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	void Update() {		
		Vector2 pos = transform.position;
		Vector2 groundPos = pos - new Vector2(0, collider2D.bounds.extents.y + .1f);
		Vector2 velocity = rigidbody2D.velocity;
		
		// check every layer except player and real
		int layerMask = ~LayerMask.GetMask("Player", "Real");
		
		var bounds = collider2D.bounds;
		var bottomLeft = bounds.min;
		var underBottomRight = new Vector2(bounds.max.x, bounds.min.y - bounds.size.y * 0.1f);
		bool grounded = Physics2D.OverlapArea(bottomLeft, underBottomRight, layerMask);
		Debug.DrawRay(pos, groundPos - pos);

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

		// Fix orientation so wolf doesn't end up upside down.
		Vector3 normal = transform.up;
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
