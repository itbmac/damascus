using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class AICharacter : MonoBehaviour {

	public float Speed = 8.0f;
	
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
	
	// Use this for initialization
	void Start () {
		
	}
	
	Vector2 GetForward() {
		if (FacingRight)
			return -transform.right;
		else
			return transform.right;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 velocity = rigidbody2D.velocity;
		
		// a position 0.1 units forward in our current direction 
		Vector2 next = (Vector2)transform.position + velocity.normalized * 0.1f;
		
		// slightly past our left or right edge (whichever one is the direction we're moving in)
		Vector2 nextSide = next + Mathf.Sign (transform.localScale.x) * new Vector2(collider2D.bounds.extents.x, 0);
		
		// a position slightly below the character's bottom of where we expect to be next
		// this is where we expect the ground to be
		Vector2 nextGround = nextSide - new Vector2(0, collider2D.bounds.extents.y + 0.1f);
		
		int layerMask = LayerMask.GetMask("Level");
		
		// if we're about to run off the ground, or run into a wall, change direction
		if (!Physics2D.Linecast(nextSide, nextGround, layerMask) || Physics2D.Linecast(next, nextSide, layerMask))
			FacingRight = !FacingRight;
		
		// draw these lines for debugging
		Debug.DrawRay(nextSide, nextGround - nextSide, Color.green);
		Debug.DrawRay(next, nextSide - next, Color.red);
		
		// larger objects should move faster, and multiplying by local scale
		// also takes into account which direction we're facing
		velocity.x = transform.localScale.x * Speed;
		rigidbody2D.velocity = velocity;
	}
}
