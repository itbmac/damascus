using UnityEngine;
using System.Collections;

public class PoliceRotation : MonoBehaviour {

	const float AngleOffset = 90;
	const float AngleTurnLengthForRepeat = 360;
	const float AngleTurnSpeed = .2f;
	const float ViceMaxOffset = 45f;
	const float ViceChange = 1;
	const float ViceMinTime = 1;
	const float ViceMaxTime = 3;
	float ViceOffset;
	float ViceOffsetChange;
	float nextChange;	
	Vector2 lastDir;
	Police police;
	
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
	
	// Use this for initialization
	void Start () {
		police = GetComponent<Police>();
	}	
	
	// Update is called once per frame
	void Update () {		
		Vector2 dir = GetComponent<PolyNavAgent>().movingDirection;
		
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
		euler.z = Mathf.LerpAngle(euler.z, angle, AngleTurnSpeed);
		transform.eulerAngles = euler;
	}
}
