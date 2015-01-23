using UnityEngine;
using System.Collections;

public class CityControls : MonoBehaviour {
	public float speed = 50f;
	public float accelRate = 0.25f;
	public float speedRatio = 5f;
	public float spriteRotateOffset = 180f;
	public float rotationAmount = 11.25f;
	int rotationFrame = 0;
	public int rotationPeriod = 3;
	public float TrafficLightSpeedChangeGreen = 7.0f;
	public float TrafficLightSpeedChangeYellow = 0.0f;
	public float TrafficLightSpeedChangeRed = 8.0f;
	public float TrafficLightMinSpeed = 1.0f;
	public bool TrafficLightSpeedChangeApplied = true;
	float TrafficLightSpeedChange = 0.0f;

	public Traffic_Light.lightColor currentLightPassed = Traffic_Light.lightColor.NONE;

	// Use this for initialization
	void Start () {

	}
	
	//update is called every frame at fixed intervals
	void FixedUpdate()
	{
		
		if (rotationFrame > 0)
			rotationFrame--;
		
		if (rotationFrame == 0) {
			if(Input.GetKey(KeyCode.RightArrow)) {
				rigidbody2D.rotation -= rotationAmount;
				rotationFrame = rotationPeriod;
			}
			
			if(Input.GetKey(KeyCode.LeftArrow)) {
				rigidbody2D.rotation += rotationAmount;
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

		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.UpArrow)) {
			float angle = Mathf.Deg2Rad * (rigidbody2D.rotation + spriteRotateOffset);
			Vector2 newVel = rigidbody2D.velocity + new Vector2(Mathf.Cos(angle) * speedRatio * accelRate, Mathf.Sin(angle) * speedRatio * accelRate);

			if (newVel.magnitude > speed) {
				rigidbody2D.velocity = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
			}
			else {
				rigidbody2D.velocity = newVel;
			}
		}
		else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			float angle = Mathf.Deg2Rad * (rigidbody2D.rotation + spriteRotateOffset);
			Vector2 newVel = rigidbody2D.velocity - new Vector2(Mathf.Cos(angle) * speedRatio * accelRate, Mathf.Sin(angle) * speedRatio * accelRate);
			
			if (newVel.magnitude > speed) {
				rigidbody2D.velocity = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
			}
			else {
				rigidbody2D.velocity = newVel;
			}
		}
	}
}
