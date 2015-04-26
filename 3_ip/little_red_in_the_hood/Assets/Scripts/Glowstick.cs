using UnityEngine;
using System.Collections;

public class Glowstick : MyMonoBehaviour {

	GameObject PoliceDepartment;
	const float CommunicationRange = 20.0f;
	const float TimeBeforeDecaying = 2;
	const float TimeToDecay = 2;

	// Use this for initialization
	void Start () {
		PoliceDepartment = GameObject.Find ("Police_Department");	
		
		foreach (Transform got in PoliceDepartment.transform) {
			var go = got.gameObject;
			float dist = Vector2.Distance(transform.position, go.transform.position);			
			if (go != gameObject && dist < CommunicationRange && !Physics2D.Linecast(transform.position, go.transform.position, LayerMask.GetMask("Obstacle"))) {
				Debug.DrawLine(transform.position, go.transform.position, Color.green, 0.5f);
				var police = go.GetComponent<Police>();
				
				police.NotifyPlayerPos(transform.position);
			}
		}
		
		StartCoroutine(DieASlowAndPainfulDeath());
	}
	
	IEnumerator DieASlowAndPainfulDeath() {
		yield return new WaitForSeconds(TimeBeforeDecaying);
		
		GameObject light = gameObject.transform.Find("light").gameObject;
		SpriteRenderer lightRenderer = light.GetComponent<SpriteRenderer>();
		float start = Time.time;
		
		while (Time.time - start < TimeToDecay) {
			float fraction = 1 - ((Time.time - start) / TimeToDecay);
			
			var newLight = lightRenderer.color;
			var newColor = color;
			
			newColor.a = fraction;	
			newLight.a = fraction;
			
			color = newColor;
			lightRenderer.color = newLight;
			
			yield return new WaitForSeconds(0.1f);
		}
		
		Player.Instance.IsOnSprayPaint = false;
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Vector2.Distance(Player.Instance.transform.position, transform.position) < RedColorBlend.glowstickRange)
		{
			Player.Instance.IsOnSprayPaint = true;
		}
	}
	
	[System.NonSerialized]
	public bool Collided = false;
	
	void OnCollisionEnter2D() {
		Collided = true;
		rigidBody2D.isKinematic = true;
	}
}
