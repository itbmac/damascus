using UnityEngine;
using System.Collections;

public class CityControls : MonoBehaviour {
	public float Speed = 1.25f;

	// Use this for initialization
	void Start () {

	}
	
	//update is called every frame at fixed intervals
	void FixedUpdate()
	{
		Vector2 newVel = GetComponent<Rigidbody2D>().velocity;
		newVel += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis ("Vertical")) * Speed;
		GetComponent<Rigidbody2D>().velocity = newVel;
	}
}
