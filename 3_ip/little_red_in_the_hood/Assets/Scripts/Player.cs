using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float WalkSpeed = 12f;
	public float SlowSpeed = 6f;
	public float RunSpeed = 18f;
	
	public GameObject PaintSplat;
	public Vector3 startLoc;
	
	public float AngleOffset = 90f;
	public float AngleTurnSpeed = .5f;
	public float IsMovingThreshold = 0.1f;

	// Use this for initialization
	void Start () {
		startLoc = transform.position;
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.P))
			Instantiate(PaintSplat, transform.position, Quaternion.identity);
	}
	
	//update is called every frame at fixed intervals
	void FixedUpdate()
	{
		float speed;
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			speed = RunSpeed;
		else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			speed = SlowSpeed;
		else
			speed = WalkSpeed;			
	
		Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
		velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
		GetComponent<Rigidbody2D>().velocity = velocity;
		
		if (velocity.magnitude > IsMovingThreshold) {
			float angle = Mathf.Repeat(Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + AngleOffset, 360F);
			Vector3 euler = transform.eulerAngles;
			euler.z = angle;
		
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), AngleTurnSpeed);
//			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, euler, AngleTurnSpeed);
		}
	}
	
	public bool IsHidden {
		get {
			return hiddenCount > 0;
		}
	}
	
	int hiddenCount;	
	void AddHidden() {
		hiddenCount += 1;
	}
	
	void RemoveHidden() {
		hiddenCount -= 1;
	}

	public void ResetPosToStart() {
		transform.position = startLoc;
	}
}
