using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	public float AngleOffset = 0;
	public bool ViceCopMode = false;

	// Use this for initialization
	void Start () {
	
	}
	
	float ViceOffset;
	float ViceOffsetChange;
	float nextChange;
	public float ViceMaxOffset = 45f;
	public float ViceChange = 1;
	public float ViceMinTime = 1;
	public float ViceMaxTime = 3;
	Vector2 lastDir;
	
	// Update is called once per frame
	void Update () {
		Vector2 dir = transform.parent.GetComponent<PolyNavAgent>().movingDirection;
	
		if (ViceCopMode) {
			if (Vector2.Distance(dir, lastDir) > 0.2f) {
				ViceOffset = 0;
				nextChange = Time.time - 1;
			}
			
			lastDir = dir;
			
			ViceOffset = Mathf.Clamp(ViceOffset + ViceOffsetChange, -ViceMaxOffset, ViceMaxOffset);
			
			if (Time.time > nextChange || ViceOffset == Mathf.Abs(ViceMaxOffset)) {
				ViceOffsetChange = Random.Range(-ViceChange, ViceChange);
				nextChange = Random.Range(ViceMinTime, ViceMaxTime) + Time.time;
			}
		}
	
		
		float angle = Mathf.Repeat (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + AngleOffset + ViceOffset, 360);
		
		Vector3 euler = transform.eulerAngles;
		euler.z = angle;
		transform.eulerAngles = euler;
	}
	
	bool playerDetectTriggered;
	void OnTriggerEnter2D(Collider2D c) {
		if (c.name == "GlowStick")
			SendMessageUpwards("OnDetectPlayer");
	
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
