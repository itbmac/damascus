using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class ArrowDirection : MyMonoBehaviour {

	public float graphicAngleOffset = 0;
	public float graphicPositionOffset = 2.5f;
	public float clipDistance = .25f;

	// Use this for initialization
	void Start () {
		collider2D.isTrigger = true; // HACK to fix arrow without changing the scene
	}
	
	// Update is called once per frame
	void Update () {			
		var target = TargetSystem.Instance.CurrentTarget;
		Vector2 tPos = target.transform.position;
		
		if (Vector2.Distance(Camera.main.WorldToViewportPoint(tPos), new Vector2(.5f, .5f)) < clipDistance) {
			spriteRenderer.enabled = false;
			return;
		} else {
			spriteRenderer.enabled = true;
		}
		
		var bounds = GetComponent<Collider2D>().bounds;
		Vector2 buffer = bounds.size * .75f;
		Vector2 cameraMax = Camera.main.ViewportToWorldPoint(Vector2.one) - (Vector3)buffer;
		Vector2 cameraMin = Camera.main.ViewportToWorldPoint(Vector2.zero) + (Vector3)buffer;
		Vector2 cameraCenter = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
		
		float offsetDistance = bounds.size.y/2 + target.GetComponent<Collider2D>().bounds.size.magnitude/2;
		var offsetDir = tPos - cameraCenter;
		// Handle the edge case of being right on the target, since we can't normalize then
		// or we'll get the zero vector which'll mess stuff up.
		if (offsetDir == Vector2.zero)
			offsetDir = Vector2.up;
		else
			offsetDir.Normalize();
		Vector2 point = tPos - offsetDistance * offsetDir.normalized;		
		
		var dir = tPos - cameraCenter;
		
		Vector3 euler = transform.eulerAngles;
		euler.z = Mathf.Repeat(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + graphicAngleOffset, 360F);
		transform.eulerAngles = euler;
		
		if (point.x > cameraMax.x || point.x < cameraMin.x || point.y > cameraMax.y || point.y < cameraMin.y) {
			point = Extensions.IntersectionWithRayFromCenter(cameraMin, cameraMax, (Vector2d)target.transform.position);
		}
			
		transform.position = point;		
	}
}
