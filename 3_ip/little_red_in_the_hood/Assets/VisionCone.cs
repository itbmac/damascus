using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	public float AngleOffset = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 dir = transform.parent.GetComponent<PolyNavAgent>().movingDirection;
		float angle = Mathf.Repeat (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + AngleOffset, 360);
		
		Vector3 euler = transform.eulerAngles;
		euler.z = angle;
		transform.eulerAngles = euler;
	}
	
	bool playerDetectTriggered;
	void OnTriggerEnter2D(Collider2D c) {
		if (c.name == "Player") {
			if (c.GetComponent<Player>().IsHidden)
				return; // TODO: what if player becomes unhidden while in cone
		
			SendMessageUpwards("OnDetectPlayer");
			playerDetectTriggered = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D c) {
		if (c.name == "Player" && !playerDetectTriggered) {
			if (c.GetComponent<Player>().IsHidden)
				return; // TODO: what if player becomes unhidden while in cone
				
			SendMessageUpwards("OnDetectPlayer");
			playerDetectTriggered = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D c) {
		if (c.name == "Player")
			playerDetectTriggered = false;
	}
}
