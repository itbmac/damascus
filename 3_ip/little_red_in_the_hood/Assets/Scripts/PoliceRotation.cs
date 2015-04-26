using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Police))]
public class PoliceRotation : MonoBehaviour {

	const float AngleOffset = 90;
	const float AngleTurnLengthForRepeat = 360;
	const float AngleTurnSpeed = .05f;
	const float ViceMaxOffset = 45f;
	const float ViceChange = 1;
	const float ViceMinTime = 1;
	const float ViceMaxTime = 3;
	float ViceOffset;
	float ViceOffsetChange;
	float nextChange;	
	Vector2 lastDir;
	Police police;
	GameObject VisionCone;
	
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
		VisionCone = transform.GetChild(0).gameObject;
	}
	
	const float VisionConeOffset = 180;
	
	void UpdateVisionCone(Vector2 dir) {
		if (!ViceCopMode) 
			return;
		
		if (Vector2.Angle(dir, lastDir) > 20f) {
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
		
		var newAngle = Mathf.Repeat(VisionConeOffset + ViceOffset + AngleTurnLengthForRepeat, AngleTurnLengthForRepeat);
		var localRotation = VisionCone.transform.localRotation;
		var localEuler = localRotation.eulerAngles;
		localEuler.z = Mathf.LerpAngle(localEuler.z, newAngle, AngleTurnSpeed);
		localRotation.eulerAngles = localEuler;		
		VisionCone.transform.localRotation = localRotation;
	}
	
	// Update is called once per frame
	void Update () {		
		if (TheGameManager.Instance.MotionStopped)
			return;
		
		Vector2 dir = GetComponent<Police>().Direction;
		
		UpdateVisionCone(dir);		
		
		float angle = Mathf.Repeat (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + AngleOffset + AngleTurnLengthForRepeat, AngleTurnLengthForRepeat);
		
		Vector3 euler = transform.eulerAngles;
		euler.z = Mathf.LerpAngle(euler.z, angle, AngleTurnSpeed);
		transform.eulerAngles = euler;
	}
}
