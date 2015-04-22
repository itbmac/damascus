using UnityEngine;
using System.Collections;

public class RedColorBlend : MyMonoBehaviour 
{
	public Color targetColor;
	public static float glowstickRange;
	public float speed;
	
	// Use this for initialization
	void Start () 
	{
		if (glowstickRange == 0)
		{
			glowstickRange = 1.0f;
		}
		
		/*
		if (speed == 0)
		{
			speed = 5.0f;
		}
		*/
		 
		if (targetColor == default(Color))
		{
			targetColor = Color.red;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		GameObject nearestGlowstick = GameObject.FindGameObjectWithTag("Glowstick");

		if (nearestGlowstick != null && 
			Vector3.Distance(nearestGlowstick.transform.position, transform.position) <= glowstickRange)
		{
			//float distToGlowstick = Vector2.Distance(transform.position, nearestGlowstick.transform.position);
			//speed = (glowstickRange - distToGlowstick) / glowstickRange;
			color = Color.red;
		}
		else
		{
			color = Color.white;
			speed = 1.0f;
		}
	}
}
