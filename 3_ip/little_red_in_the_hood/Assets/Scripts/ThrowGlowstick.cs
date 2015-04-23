﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThrowGlowstick : MonoBehaviour 
{
	public float GlowstickScale = 0.5f;
	public float MoveSpeed = 20f;
	public int RotationSpeed;

	enum States {Throwing, Holding, Stowed}
	
	States state = States.Stowed;
	Vector2 source;
	Vector2 target;
	GameObject playerObject;
	Player playerScript;
	GameObject glowstickPrefab;
	Text text;
	
	// Use this for initialization
	void Start () 
	{		
		playerScript = Player.Instance;
		playerObject = playerScript.gameObject;
		text = GetComponentInChildren<Text>();
		glowstickPrefab = (GameObject)Resources.Load("GlowStick");
		
		text.text = playerScript.NumGlowsticks.ToString();
	}
	
	public void ToggleHoldGlowstick () 
	{
		if (state == States.Holding)
		{
			state = States.Stowed;
		}
		else if (state == States.Stowed)
		{
			state = States.Holding;
		}
	}
	
	IEnumerator ThrowGlowstickCoroutine ()
	{
		playerScript.NumGlowsticks -= 1;

		Vector2 source = playerObject.transform.position;
		Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		state = States.Stowed;
		
		Quaternion rotation = Quaternion.AngleAxis(RotationSpeed, Vector3.forward * Random.Range(0.0f, 1.0f));
		GameObject glowstick = (GameObject)GameObject.Instantiate(glowstickPrefab, source, rotation);
		glowstick.transform.localScale *= 0.5f;
		
		while (Vector2.Distance(glowstick.transform.position, target) >= 0.5f)
		{
			//Slow movement as we near the target
			float distTraveled = Vector2.Distance(glowstick.transform.position, source);
			float dist = Vector2.Distance(source, target);
			float percentDone = distTraveled / dist;
			float step = MoveSpeed * Time.deltaTime * (1 - percentDone);
			
			//Movement
			glowstick.transform.position = Vector2.MoveTowards(glowstick.transform.position, target, step);
			glowstick.transform.Rotate(Vector3.forward * (RotationSpeed * (1 - percentDone)));
			
			yield return new WaitForSeconds(0.01f);
		}
		
		yield break;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerScript.NumGlowsticks <= 0)
		{
			state = States.Stowed;
		}
		else if (state == States.Holding && Input.GetMouseButtonDown(0))
		{
			state = States.Throwing;
			StartCoroutine(ThrowGlowstickCoroutine());
		}
		
		text.text = playerScript.NumGlowsticks.ToString();
	}
}
