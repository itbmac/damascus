﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThrowGlowstick : MonoBehaviour 
{
	enum States {throwing, holding, stowed};
	States state;
	Vector2 source;
	Vector2 target;
	GameObject playerObject;
	Player playerScript;
	Object glowstickPrefab;
	public float glowstickScale;
	public float moveSpeed;
	public int rotationSpeed;
	Text text;
	
	// Use this for initialization
	void Start () 
	{
		if (moveSpeed == 0)
		{
			moveSpeed = 20.0f;
		}
		
		if (glowstickScale == 0)
		{
			glowstickScale = 0.5f;
		}
		
		playerObject = GameObject.FindGameObjectWithTag("Player");
		playerScript = playerObject.GetComponent<Player>();
		state = States.stowed;
		text = GameObject.Find("Text").GetComponent<Text>();
		glowstickPrefab = Resources.Load("GlowStick");
		
		text.text = playerScript.NumGlowsticks.ToString();
	}
	
	public void ToggleHoldGlowstick () 
	{
		if (state == States.holding)
		{
			state = States.stowed;
		}
		else if (state == States.stowed)
		{
			state = States.holding;
		}
	}
	
	IEnumerator ThrowGlowstickCoroutine ()
	{
		playerScript.NumGlowsticks -= 1;

		Vector2 source = playerObject.transform.position;
		Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		state = States.stowed;
		
		Quaternion rotation = Quaternion.AngleAxis(rotationSpeed, Vector3.forward * Random.Range(0.0f, 1.0f));
		GameObject glowstick = (GameObject)GameObject.Instantiate(glowstickPrefab, source, rotation);
		glowstick.transform.localScale *= 0.5f;
		
		while (Vector2.Distance(glowstick.transform.position, target) >= 0.5f)
		{
			//Slow movement as we near the target
			float distTraveled = Vector2.Distance(glowstick.transform.position, source);
			float dist = Vector2.Distance(source, target);
			float percentDone = distTraveled / dist;
			float step = moveSpeed * Time.deltaTime * (1 - percentDone);
			
			//Movement
			glowstick.transform.position = Vector2.MoveTowards(glowstick.transform.position, target, step);
			glowstick.transform.Rotate(Vector3.forward * (rotationSpeed * (1 - percentDone)));
			
			yield return null;
		}
		
		yield break;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerScript.NumGlowsticks <= 0)
		{
			state = States.stowed;
		}
		else if (state == States.holding && Input.GetMouseButtonDown(0))
		{
			state = States.throwing;
			StartCoroutine(ThrowGlowstickCoroutine());
		}
		
		text.text = playerScript.NumGlowsticks.ToString();
	}
}
