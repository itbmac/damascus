using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MyMonoBehaviour {
	public float WalkSpeed = 12f;
	public float SlowSpeed = 6f;
	public float RunSpeed = 18f;
	public float IdleSpeed = 1f;

	public GameObject PaintSplat;
	public GameObject Glowstick;
	
	public float AngleOffset = 90f;
	public float AngleTurnSpeed = .5f;
	public float IsMovingThreshold = 0.1f;
	
	public int NumPaint = 5;
	public int NumGlowsticks = 5;
	
	public float Health = 1.0f;

	public AudioClip WalkSound;
	public AudioClip RunSound;
	public AudioClip StealthSound;
	
	public static Player Instance {
		get; private set;
	}
	
	public bool IsUnderStreetlight {
		get; set;
	}
	
	public bool IsOnSprayPaint {
		get; set;
	}
	
	const string MovementMode = "MovementMode";
	enum AnimState {
		Idle = 0,
		Sneak = 1,
		Walk = 2,
		Run = 3		
	}
	
	SpriteRenderer redDiscoveredRenderer;
	public float HealthRegenRate = 0.1f;
	
	private bool GodMode;
	public bool StealthMode {
		get; private set;
	}
	
	bool gameOver;
	
	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		redDiscoveredRenderer = transform.Find("red_discovered").GetComponent<SpriteRenderer>();
	}
	
	void Update() {
		if (TheGameManager.Instance.MotionStopped)
			return;
	
		IsUnderStreetlight = GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Streetlight"));	
		IsOnSprayPaint = GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("SprayPaint"));	
		
		if (IsOnSprayPaint) {
			color = Color.red;
		} else if (IsUnderStreetlight) {
			color = Color.yellow;
		} else {
			color = Color.white;
		}
		
		Color newColor = redDiscoveredRenderer.color;
		newColor.a = 1.0f - Health;
		redDiscoveredRenderer.color = newColor;
		
		if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.M) && Input.anyKeyDown) {
			GodMode = !GodMode;
			if (GodMode)
				print("God mode on");
			else
				print("God mode off");
		}
		
		if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.M) && Input.anyKeyDown) {
			StealthMode = !StealthMode;
			if (StealthMode)
				print("Stealth mode on");
			else
				print("Stealth mode off");
		}
		
		if (!gameOver && !GodMode) {
			if (collider2D.IsTouchingLayers(LayerMask.GetMask("Police"))) {
				gameOver = true;
				FindObjectOfType<FadeToBlack>().Trigger();
				return;
			}
		}
		Health += HealthRegenRate * Time.deltaTime;
		Health = Mathf.Min(1, Health);
	}
	
	//update is called every frame at fixed intervals
	void FixedUpdate()
	{
		if (TheGameManager.Instance.MotionStopped) {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			anim.SetInteger(MovementMode, (int)AnimState.Idle);
			return;
		}
	
		float speed;
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
			speed = RunSpeed;
		} else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
			speed = SlowSpeed;
		} else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
			speed = WalkSpeed;			
		} else {
			speed = IdleSpeed;
		}

		Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
		float velSpeed = velocity.magnitude;

		if (velSpeed > WalkSpeed) {
			anim.SetInteger(MovementMode, (int)AnimState.Run);
			audio.clip = RunSound;
		} else if (velSpeed > SlowSpeed) {
			anim.SetInteger(MovementMode, (int)AnimState.Walk);
			audio.clip = WalkSound; 
		} else if (velSpeed > 0){
			anim.SetInteger(MovementMode, (int)AnimState.Sneak);
			audio.clip = StealthSound;
		} else {
			anim.SetInteger(MovementMode, (int)AnimState.Idle);
			audio.Stop();
			audio.clip = null;
		}
	
		if (!audio.isPlaying && audio.clip != null)
			audio.Play ();

		velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
		GetComponent<Rigidbody2D>().velocity = velocity;
		
		if (velocity.magnitude > IsMovingThreshold) {
			float angle = Mathf.Repeat(Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + AngleOffset, 360F);
			Vector3 euler = transform.eulerAngles;
			euler.z = angle;
		
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), AngleTurnSpeed);
		}
	}
}
