using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float WalkSpeed = 12f;
	public float SlowSpeed = 6f;
	public float RunSpeed = 18f;
	
	public GameObject PaintSplat;
	public GameObject Glowstick;
	public Vector3 startLoc;
	
	public float AngleOffset = 90f;
	public float AngleTurnSpeed = .5f;
	public float IsMovingThreshold = 0.1f;
	
	public int NumPaint = 5;
	public int NumGlowsticks = 5;
	
	public static Player Instance {
		get; private set;
	}
	
	Collider2D collider2D;
	
	public bool IsUnderStreetlight {
		get; private set;
	}
	
	void Awake() {
		Instance = this;
		collider2D = GetComponent<Collider2D>();
	}

	// Use this for initialization
	void Start () {
		startLoc = transform.position;
	}
	
	void Update() {
		IsUnderStreetlight = collider2D.IsTouchingLayers(LayerMask.GetMask("Streetlight"));	
	
		if (Input.GetKeyDown(KeyCode.P) && NumPaint > 0) {
			NumPaint -= 1;
			Instantiate(PaintSplat, transform.position, Quaternion.identity);
		}
		
		if (Input.GetKeyDown(KeyCode.G) && NumGlowsticks > 0) {
			NumGlowsticks -= 1;
			Instantiate(Glowstick, transform.position, Quaternion.identity);
		}
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
