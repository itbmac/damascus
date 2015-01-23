using UnityEngine;
using System.Collections;

public class IndyControls : MonoBehaviour {
	public float speed = 10f;
	public float speedInWall = 4f;
	public float accelRate = 0.1f;
	public float drag = 20f;
	public float dragInWall = 30f;
	public float mass = 10f;
	public float gravityScale = 0f;
	public float spriteRotateOffset = 180f;
	public float TrafficLightSpeedChangeGreen = 8.0f;
	public float TrafficLightSpeedChangeYellow = 0.0f;
	public float TrafficLightSpeedChangeRed = 8.0f;
	public float TrafficLightMinSpeed = 1.0f;
	public bool TrafficLightSpeedChangeApplied = true;
	float TrafficLightSpeedChange = 0.0f;
	int rotationFrame = 0;
	int rotationPeriod = 4;
	float speedRatio;
	
	public bool inWallMaterial = false;
	public Traffic_Light.lightColor currentLightPassed = Traffic_Light.lightColor.NONE;

	// Use this for initialization
	void Start () {
		//The degree to which this object is affected by gravity.
		rigidbody2D.gravityScale = gravityScale;
		
		//The rigidBody inertia.
		rigidbody2D.drag = drag;
		
		//Mass of the rigidbody.
		rigidbody2D.mass = mass;

		speedRatio = speed/speedInWall;
	}
	
	//update is called every frame at fixed intervals
	void FixedUpdate()
	{
		
		if (rotationFrame > 0)
			rotationFrame--;
		
		if (rotationFrame == 0) {
			if(Input.GetKey(KeyCode.RightArrow)) {
				rigidbody2D.rotation -= 22.5f;
				rotationFrame = rotationPeriod;
			}
			
			if(Input.GetKey(KeyCode.LeftArrow)) {
				rigidbody2D.rotation += 22.5f;
				rotationFrame = rotationPeriod;
			}
		}

		if (!TrafficLightSpeedChangeApplied) {

			if (currentLightPassed == Traffic_Light.lightColor.RED) {
				TrafficLightSpeedChange = TrafficLightSpeedChangeRed;

				float angle = Mathf.Deg2Rad * (rigidbody2D.rotation + spriteRotateOffset);
				Vector2 newVel = rigidbody2D.velocity - new Vector2(Mathf.Cos(angle) * TrafficLightSpeedChange, Mathf.Sin(angle) * TrafficLightSpeedChange);

				if (newVel.magnitude < TrafficLightMinSpeed) {
					rigidbody2D.velocity = new Vector2(Mathf.Cos(angle) * TrafficLightMinSpeed, Mathf.Sin(angle) * TrafficLightMinSpeed);
				}
				else {
					rigidbody2D.velocity = newVel;
				}
			}
			else if (currentLightPassed == Traffic_Light.lightColor.YELLOW) {
				TrafficLightSpeedChange = TrafficLightSpeedChangeYellow;
			}
			else if (currentLightPassed == Traffic_Light.lightColor.GREEN) {
				TrafficLightSpeedChange = TrafficLightSpeedChangeGreen;

				float angle = Mathf.Deg2Rad * (rigidbody2D.rotation + spriteRotateOffset);
				rigidbody2D.velocity += new Vector2(Mathf.Cos(angle) * TrafficLightSpeedChange, Mathf.Sin(angle) * TrafficLightSpeedChange);
			}
			else {
				TrafficLightSpeedChange = 0.0f;
			}

			TrafficLightSpeedChangeApplied = true;
		}

		if (inWallMaterial) {
			if(Input.GetKey(KeyCode.A)) {
				float angle = Mathf.Deg2Rad * (rigidbody2D.rotation + spriteRotateOffset);
				Vector2 newVel = rigidbody2D.velocity + new Vector2(Mathf.Cos(angle) * accelRate, Mathf.Sin(angle) * accelRate);

				if (newVel.magnitude > speedInWall) {
					rigidbody2D.velocity = new Vector2(Mathf.Cos(angle) * speedInWall, Mathf.Sin(angle) * speedInWall);
				}
				else {
					rigidbody2D.velocity = newVel;
				}
			}
			rigidbody2D.drag = dragInWall;
		}
		else {
			if(Input.GetKey(KeyCode.A)) {
				float angle = Mathf.Deg2Rad * (rigidbody2D.rotation + spriteRotateOffset);
				Vector2 newVel = rigidbody2D.velocity + new Vector2(Mathf.Cos(angle) * speedRatio * accelRate, Mathf.Sin(angle) * speedRatio * accelRate);

				if (newVel.magnitude > speed) {
					rigidbody2D.velocity = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
				}
				else {
					rigidbody2D.velocity = newVel;
				}
			}

			rigidbody2D.drag = drag;
		}
	}

}
