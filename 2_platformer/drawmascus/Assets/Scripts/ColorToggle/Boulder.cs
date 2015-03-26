using UnityEngine;
using System.Collections;
using System.Linq;

public class Boulder : MonoBehaviour {

	public GameObject PathContainer;
	public float TargetRadius = 2.0f;
	public float TargetSpeed = 2.0f;
	
	Transform[] path;
	int pathIndex;
	bool activated;

	// Use this for initialization
	void Start () {
		path = PathContainer.transform.Cast<Transform>().ToArray();
		pathIndex = 0;
		activated = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (pathIndex > path.Length || !activated)
			return;
	
		Transform currentTarget = path[pathIndex];
	
		Vector2 vel = rigidbody2D.velocity;
		if (Mathf.Abs(vel.x) < TargetSpeed) {
			float delta = 0.2f * Mathf.Sign(currentTarget.position.x - transform.position.x);
			vel.x += delta;
		}
		
		rigidbody2D.velocity = vel;
		
		float dist = Vector2.Distance(transform.position, currentTarget.position);
		if (dist < TargetRadius)
			pathIndex += 1;
	
//		print(currentTarget.name + " " + dist);
	}
	
	[Signal]
	void Activate() {
		activated = true;
	}
}
