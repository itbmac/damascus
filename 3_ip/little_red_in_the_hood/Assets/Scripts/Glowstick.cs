using UnityEngine;
using System.Collections;

public class Glowstick : MonoBehaviour {

	const float CommunicationRange = 20.0f;

	// Use this for initialization
	void Start () {
		var PoliceDepartment = GameObject.Find ("Police_Department");	
	
		foreach (Transform got in PoliceDepartment.transform) {
			var go = got.gameObject;
			float dist = Vector2.Distance(transform.position, go.transform.position);			
			if (go != gameObject && dist < CommunicationRange && !Physics2D.Linecast(transform.position, go.transform.position, LayerMask.GetMask("Obstacle"))) {
				Debug.DrawLine(transform.position, go.transform.position, Color.green, 0.5f);
				var police = go.GetComponent<Police>();
				police.NotifyPlayerPos(transform.position);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
