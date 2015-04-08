using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	public float AngleOffset = 0;
	const float AngleTurnLengthForRepeat = 360;
	public float AngleTurnSpeed = .5f;
	
	bool ViceCopMode {
		get {
			return police.ViceCopMode || police.CurrentState != Police.State.Normal;
		}
	}
	
	bool InvestigativeMode {
		get {
			return police.CurrentState != Police.State.Normal;
		}
	}
	
	Police police;

	// Use this for initialization
	void Start () {
		police = GetComponentInParent<Police>();
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
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		
		if (InvestigativeMode) {
			sr.color = Color.red;
		} else {
			sr.color = Color.white;
		}
			
	
		Vector2 dir = transform.parent.GetComponent<PolyNavAgent>().movingDirection;
	
		if (ViceCopMode) {
			if (Vector2.Distance(dir, lastDir) > 0.2f) {
				ViceOffset = 0;
				nextChange = Time.time - 1;
			}
			
			lastDir = dir;
			
			ViceOffset = Mathf.Clamp(ViceOffset + ViceOffsetChange, -ViceMaxOffset, ViceMaxOffset);
			
			if (Time.time > nextChange || ViceOffset >= Mathf.Abs(ViceMaxOffset)) {
				ViceOffsetChange = Random.Range(-ViceChange, ViceChange);
				if (InvestigativeMode)
					ViceOffsetChange *= 3;
				
				nextChange = Random.Range(ViceMinTime, ViceMaxTime) + Time.time;
			}
		}
	
		
		float angle = Mathf.Repeat (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + AngleOffset + ViceOffset, AngleTurnLengthForRepeat);
		
		Vector3 euler = transform.eulerAngles;
		euler.z = angle;
		transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, euler, AngleTurnSpeed);
	}
	
//	bool playerDetectTriggered;
//	void OnTriggerEnter2D(Collider2D c) {
//		if (c.name == "GlowStick")
//			SendMessageUpwards("OnDetectPlayer");
//	
//		if (c.name == "Player") {
//			if (c.GetComponent<Player>().IsHidden)
//				return; // TODO: what if player becomes unhidden while in cone
//		
//			SendMessageUpwards("OnDetectPlayer");
//			playerDetectTriggered = true;
//		}
//	}
	
//	void OnTriggerStay2D(Collider2D c) {
//		if (c.name == "Player" && !playerDetectTriggered) {
//			if (c.GetComponent<Player>().IsHidden)
//				return; // TODO: what if player becomes unhidden while in cone
//				
//			SendMessageUpwards("OnDetectPlayer");
//			playerDetectTriggered = true;
//		}
//	}
//	
//	void OnTriggerExit2D(Collider2D c) {
//		if (c.name == "Player")
//			playerDetectTriggered = false;
//	}
}
