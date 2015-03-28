using UnityEngine;
using System.Collections;

public class CityControls : MonoBehaviour {
	public float speed = 50f;
	public float accelRate = 0.25f;
	public float speedRatio = 5f;
	public float spriteRotateOffset = 180f;
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
		Vector2 newVel = GetComponent<Rigidbody2D>().velocity;

		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			newVel += new Vector2(0.0f, Mathf.Min(speed, speedRatio * accelRate));
		}

		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			newVel += new Vector2(Mathf.Max(-1 * speed, -1 * speedRatio * accelRate), 0.0f);
		}

		if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			newVel += new Vector2(0.0f, Mathf.Max(-1 * speed, -1 * speedRatio * accelRate));
		}

		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			newVel += new Vector2(Mathf.Min(speed, speedRatio * accelRate), 0.0f);
		}

		GetComponent<Rigidbody2D>().velocity = newVel;
	}
}
