using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ColorToggle))]
public class Butterfly : Colorable {

	public GameObject PathContainer;
	Transform currentTarget;
	Transform current;
	int currentIndex;
	Transform[] path;

	public float Speed = 15.0f;

	private Vector2 CurrentDirection;
	private float NextChangeTime;
	private GameObject PlayerObject;
	private bool Ensnared;
	private float EnsnareMultiplier = 1.0f;
	
	Animator anim;

	private bool isActing = false;
	
	public AudioClip Sound;
	
	void Awake() {
		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		PlayerObject = Player.Instance.gameObject;
		path = PathContainer.transform.Cast<Transform>().ToArray();
		
		currentTarget = path.MinBy(x => Vector2.Distance(x.transform.position, transform.position));
		current = currentTarget;
	}
	
	float lastUpdate;
	
	void UpdateCurrentTarget() {
		if (Time.time - lastUpdate < 0.1f)
			return;
		lastUpdate = Time.time;
	
		Vector2 playerPos = PlayerObject.transform.position;
		
		var origin = collider2D.bounds.center;
		var radius = ((CircleCollider2D)collider2D).radius * .8f;
		
		var newTargets = new List<Transform>(); 
		
		var layerMask = 1; // LayerMask.GetMask("Default") incorrectly returns 0
		
		int index = path.ToList().FindIndex(x => x == current);
		
//		var current = path.MinBy(x => Vector2.Distance(x.transform.position, transform.position));
		var choices = new List<Transform>() {
			path[(index - 1 + path.Count()) % path.Count()],
			current,
			path[(index + 1) % path.Count()]
		};
		
		currentTarget = choices.Where (
			x => !Physics2D.CircleCast(
			origin, 
			radius, 
			x.position - collider2D.bounds.center,
			(x.position - collider2D.bounds.center).magnitude,
			layerMask).collider
		).MaxBy(x => Vector2.Distance(x.transform.position, playerPos));
		
//		foreach (var x in path) {
//			var diff = x.position - collider2D.bounds.center;
//			
//			var col = Physics2D.CircleCast(
//				origin, 
//				radius, 
//				diff.normalized,
//				diff.magnitude,
//				layerMask).collider;
//			
//			if (col)
//				Debug.DrawRay(origin, diff, Color.red);
//			else
//				Debug.DrawRay(origin, diff, Color.blue);
//		
//			if (null == col)
//				newTargets.Add(x);
//			else
//				print ("cant get " + x.name);
//		}
//		
//		if (newTargets.Count() > 0) {
//			print ("updated!");
//			currentTarget = newTargets.OrderBy(x => Vector2.Distance(x.position, transform.position)).Take(3).MaxBy(x => Vector2.Distance(x.transform.position, playerPos));
//		}
		
//		if (Vector2.Distance(playerPos, nextClosest.position) < Vector2.Distance(playerPos, currentTarget.position))
//			currentTarget = nextClosest;
	}
	
	// Update is called once per frame
	void Update () {
		if (Ensnared)
			return;
			
		if (Vector2.Distance(transform.position, currentTarget.position) < 0.4f)
			current = currentTarget;
			
		UpdateCurrentTarget();
		
//		if (Time.time > NextChangeTime)
//			ChangeDirection();

		var direction = (Vector2)(currentTarget.position - transform.position);
		if (direction.sqrMagnitude > 1)
			direction.Normalize();
			
		EnsnareMultiplier = Mathf.Clamp01(EnsnareMultiplier * 1.01f);
			
		Vector2 playerAvoidVelocity = transform.position - PlayerObject.transform.position;
		float distanceToPlayer = playerAvoidVelocity.magnitude;
		playerAvoidVelocity /= distanceToPlayer;
		playerAvoidVelocity *= 3.0f / (distanceToPlayer*distanceToPlayer);
		
		rigidbody2D.velocity = (0.5f * direction + 0.5f * playerAvoidVelocity) * Speed * EnsnareMultiplier;
		
		Debug.DrawLine(transform.position, currentTarget.position);
		
//		Vector3 eulerAngles = transform.eulerAngles;
//		if (eulerAngles.z > 180f) eulerAngles.z -= 360f;
//		eulerAngles.z = Mathf.Lerp(eulerAngles.z, 0, 0.1f); //Mathf.Clamp(eulerAngles.z, -45, 45);
//		if (eulerAngles.z < 0) eulerAngles.z += 360f;
//		
//		transform.eulerAngles = eulerAngles;
	}
	
	void ChangeDirection() {
		CurrentDirection = Random.insideUnitCircle;
		if (CurrentDirection == Vector2.zero)
			CurrentDirection = Vector2.up;
		CurrentDirection.Normalize();
		
		NextChangeTime = Time.time + Random.Range(.25f, 2f);
	}
	
	public override void OnTurnReal() {
		anim.SetBool("IsDrawing", false);
	}
	
	public override void OnTurnDrawing() {
		anim.SetBool("IsDrawing", true);		
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		CurrentDirection = coll.contacts[0].normal;
	
//		ChangeDirection();
	}
	
	public void Ensnare() {
		EnsnareMultiplier *= 0.9f;
		if (EnsnareMultiplier < 0.01f) {
			rigidbody2D.isKinematic = true;
			rigidbody2D.velocity = Vector2.zero;
			Ensnared = true;
		}
	}
}
