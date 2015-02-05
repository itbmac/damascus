using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (AudioSource))]
public class Status : MonoBehaviour 
{
	UI batteries;
	AudioClip enemySound;
	HashSet<GameObject> triggers;
	public float dist;
	
	// Use this for initialization
	void Start () 
	{
		batteries = gameObject.GetComponent<UI>();
		audio.clip = (AudioClip)Resources.Load("Enemy sound");
		triggers = new HashSet<GameObject>();
	}
	
	void OnTriggerEnter (Collider other)
	{
		triggers.Add(other.gameObject);
	}
	
	void OnTriggerExit (Collider other)
	{
		triggers.Remove(other.gameObject);
	}
	
	void OnTriggerStay (Collider other)
	{
		if (other.name == "Enemy")
		{
			if (Vector3.Distance(gameObject.transform.position, other.transform.position) < dist)
			{
				//audio.volume = 1.0f;
				audio.Play();
				StartCoroutine(damageOnStay(other.transform.position));
			}
		}
		else if (other.name == "Battery")
		{
			batteries.addBattery();
			EchoLocation.reference.removeObjectFromEcho(other.gameObject);
			Destroy(other.gameObject);
		}
		else if (other.name == "Goal")
		{ 
			levelClear();
		}
	}
	
	IEnumerator damageOnStay (Vector3 pos)
	{
		float start = 0.0f;
		
		while ((start += Time.deltaTime) <= 0.4f)
		{
			yield return null;
		}
		
		if (Vector3.Distance(gameObject.transform.position, pos) < 0.5f)
		{
			batteries.removeBattery();
		}
		else
		{
			//audio.Stop();
		}
	}
	
	void levelClear()
	{
		
	}
}
