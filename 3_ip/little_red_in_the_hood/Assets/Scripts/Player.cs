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
	public GameObject Regions;
	
	public float AngleOffset = 90f;
	public float AngleTurnSpeed = .5f;
	public float IsMovingThreshold = 0.1f;
	
	public int NumPaint = 5;
	public int NumGlowsticks = 5;
	
	public float Health = 1.0f;

	public AudioClip WalkSound;
	public AudioClip RunSound;
	public AudioClip StealthSound;
	public AudioClip ArrestedSound;
	public AudioClip ArrestedWilliamSound;
	
	public static Player Instance {
		get; private set;
	}
	
	bool _isUnderStreetLight;
	public bool IsUnderStreetlight {
		get {
			return _isUnderStreetLight;
		}
		
		set {
			if (value == _isUnderStreetLight)
				return;
			_isUnderStreetLight = value;
			
			if (value)
				streetLampFlicker.Play();
			else
				streetLampFlicker.Stop();
		}
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
	
	public string FindCurrentRegion()
	{
		RaycastHit2D[] hits;
		hits = Physics2D.RaycastAll(transform.position, Vector3.down);
		
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.transform.parent != null)
			{
				if (hit.transform.parent.name == Regions.name)
				{
					return hit.transform.name;
				}
			}
		}
		
		return "";
	}
	
	AudioSource streetLampFlicker;
	
	// Use this for initialization
	void Start () {
		redDiscoveredRenderer = transform.Find("red_discovered").GetComponent<SpriteRenderer>();
		Regions = GameObject.Find("Regions");
		streetLampFlicker = transform.Find("StreetLampFlicker").GetComponent<AudioSource>();
	}
	
	bool SpeedupMode;
	
	void UpdateCheats() {
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
		
		// NC = noclip, disable collisions on player, walk through anything
		if (Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.C) && Input.anyKeyDown) {
			collider2D.enabled = !collider2D.enabled;
			
			if (!collider2D.enabled)
				print("noclip on");
			else
				print ("noclip off");
		}
		
		if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.U) && Input.anyKeyDown) {
			SpeedupMode = !SpeedupMode;
			
			if (SpeedupMode)
				print("speedup on");
			else
				print ("speedup off");
		}
	}
	
	void Update() {
		if (TheGameManager.Instance.MotionStopped)
			return;
	
		IsUnderStreetlight = GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Streetlight"));	
		IsOnSprayPaint = GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("SprayPaint"));	
		
		if (IsOnSprayPaint) {
			color = Color.red;
		} else if (IsUnderStreetlight) {
			color = Color.white;
		} else {
			color = new Color(0.7f,0.7f,0.7f); //Color.gray;
		}
		
		Color newColor = redDiscoveredRenderer.color;
		newColor.a = 1.0f - Health;
		redDiscoveredRenderer.color = newColor;
		
		UpdateCheats();
		
		if (!gameOver && !GodMode) {
			if (collider2D.IsTouchingLayers(LayerMask.GetMask("Police"))) {
				gameOver = true;
				FindObjectOfType<FadeToBlack>().Trigger();
				
				if (collider2D.IsTouching(GameObject.Find("William").GetComponent<Collider2D>()))
					audio.clip = ArrestedWilliamSound;
				else
					audio.clip = ArrestedSound;
					
				audio.loop = false;
				audio.Play();
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
		
		if (SpeedupMode)
			speed *= 5;

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
	
		if (!audio.isPlaying && audio.clip != null) {
			audio.loop = true;
			audio.Play ();
		}

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
