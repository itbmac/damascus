using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Butterfly : Colorable {

	private float Speed = 5.0f;

	private Vector2 CurrentDirection;
	private float NextChangeTime;
	public GameObject PlayerObject;

	// Use this for initialization
	void Start () {
		PlayerObject = Player.Instance.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > NextChangeTime)
			ChangeDirection();
			
		Vector2 playerAvoidVelocity = transform.position - PlayerObject.transform.position;
		float distanceToPlayer = playerAvoidVelocity.magnitude;
		playerAvoidVelocity /= distanceToPlayer;
		playerAvoidVelocity *= 3.0f / distanceToPlayer*distanceToPlayer;
		
		rigidbody2D.velocity = (0.5f * CurrentDirection + 0.5f * playerAvoidVelocity) * Speed;
		
		Vector3 eulerAngles = transform.eulerAngles;
		if (eulerAngles.z > 180f) eulerAngles.z -= 360f;
		eulerAngles.z = Mathf.Lerp(eulerAngles.z, 0, 0.1f); //Mathf.Clamp(eulerAngles.z, -45, 45);
		if (eulerAngles.z < 0) eulerAngles.z += 360f;
		
		transform.eulerAngles = eulerAngles;
	}
	
	void ChangeDirection() {
		Debug.Log ("change");
		CurrentDirection = Random.insideUnitCircle;
		if (CurrentDirection == Vector2.zero)
			CurrentDirection = Vector2.up;
		CurrentDirection.Normalize();
		
		NextChangeTime = Time.time + Random.Range(.25f, 2f);
	}
	
	protected override void Activate() {
		
	}
	
	protected override void Deactivate() {
		
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		CurrentDirection = coll.contacts[0].normal;
	
//		ChangeDirection();
	}
}
