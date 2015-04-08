using UnityEngine;
using System.Collections;

public class Glowstick : MyMonoBehaviour {

	const float CommunicationRange = 20.0f;
	const float TimeBeforeDecaying = 2;
	const float TimeToDecay = 2;

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
		
		StartCoroutine(DieASlowAndPainfulDeath());
	}
	
	IEnumerator DieASlowAndPainfulDeath() {
		yield return new WaitForSeconds(TimeBeforeDecaying);
		
		float start = Time.time;
		while (Time.time - start < TimeToDecay) {
			float fraction = 1 - ((Time.time - start) / TimeToDecay);
			
			var newColor = color;
			newColor.a = fraction;			
			color = newColor;
			
			yield return new WaitForSeconds(0.1f);
		}
		
		
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
