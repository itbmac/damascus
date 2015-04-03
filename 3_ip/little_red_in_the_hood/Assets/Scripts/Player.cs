using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float Speed = 1.25f;
	public GameObject PaintSplat;
	public Vector3 startLoc;

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
		Vector2 newVel = GetComponent<Rigidbody2D>().velocity;
		newVel += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis ("Vertical")) * Speed;
		GetComponent<Rigidbody2D>().velocity = newVel;
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
