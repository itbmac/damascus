using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (SphereCollider))]
[RequireComponent (typeof (Rigidbody))]
public class EchoLocation : MonoBehaviour 
{
	public float speed = 17.0f;
	public float maxDist = 12.0f;
	public Material lightupTexture;
	public float timeToLightup = 0.5f;
	public float timeToFade = 1.0f;
	public bool done = false;
	public SphereCollider sphereCollider;
	public HashSet<GameObject> toLight;
	public AudioClip sound;
	public static EchoLocation reference;
	public float timeLocked = 0.0f;
	public float timeToLock = 0.4f;
	public bool locked = false;
	
	// Use this for initialization
	void Awake () 
	{
		sphereCollider = this.GetComponent<SphereCollider>();
		sphereCollider.radius = maxDist;
		sphereCollider.isTrigger = true;
		toLight = new HashSet<GameObject>();
		sound = (AudioClip)Resources.Load("sonar ping");
		reference = this;
	}
	
	public void removeObjectFromEcho (GameObject obj)
	{
		toLight.Remove(obj);
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.name == "Sphere")
		{
			toLight.Add(other.gameObject);
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		toLight.Remove(other.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeLocked += Time.deltaTime;
	}
	
	public void ping(bool decrement)
	{	
		if (timeLocked >= timeToLock)
		{
			timeLocked = 0.0f;
			locked = false;
		}
		if (UI.reference.batteryBar.batteries > 1 && !locked)
		{
			locked = true;
			
			if (decrement)
			{
				UI.reference.removeBattery();
			}
			
			print(UI.reference.batteryBar.batteries);
			AudioSource.PlayClipAtPoint(sound, gameObject.transform.position);
			
			print ("ping");
			foreach (GameObject obj in toLight)
			{
				StartCoroutine(waveLightUp(obj));
			}
		}
	}
	
	IEnumerator waveLightUp (GameObject obj)
	{
		if (obj != null)
		{
			Color oldColor = obj.renderer.material.color;
			oldColor.a = 0.0f;
			Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.0f);
			
			float dist = Vector3.Distance(this.gameObject.transform.position, obj.gameObject.transform.position);
			float timeLeft = dist/speed/4.0f;
			
			while ((timeLeft -= Time.deltaTime) > 0.0f)
			{
				yield return null;
			}
			
			float startT = 0;
			float iter = 1.0f/timeToLightup/30.0f;
			
			//wait for the sphere to lightup
			while ((startT += iter) < 1)
			{
				newColor.a += iter;
				obj.renderer.material.color = newColor;
				yield return null; 
			}
			
			startT = 1.0f; 
			iter = 1.0f/timeToFade/30.0f;
			
			//wait for it to fade
			while ((startT -= iter) > 0)
			{
				newColor.a -= iter;
				obj.renderer.material.color = newColor;
				yield return null;
			}
			
			obj.renderer.material.color = oldColor;
		}
	}
}
