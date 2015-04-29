using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
public class CircleNotifier : MonoBehaviour {

	public float MaxScale = 10.0f;
	public float ScaleSpeed = 0.1f;

	private float scale;
	private float LastSeenTime;
	
	[System.NonSerialized]
	public Vector2 LastSeenPosition;

	// Use this for initialization
	void Start () {
		scale = 0.0f;
		transform.localScale = new Vector3(scale,scale,scale);
		LastSeenTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		scale += ScaleSpeed;
		transform.localScale = new Vector3(scale,scale,scale);
		
		if (scale > MaxScale) {
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		var police = other.GetComponent<Police>();
		if (police) {
			police.NotifyPlayerPos(LastSeenPosition, LastSeenTime);
		}
	}
}
