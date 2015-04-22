using UnityEngine;
using System.Collections;

public class Redlight : MonoBehaviour 
{
	CircleCollider2D collider;
	
	// Use this for initialization
	void Start () 
	{
		collider = gameObject.GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Vector2.Distance(Player.Instance.transform.position, transform.position) < collider.radius)
		{
			Player.Instance.IsOnSprayPaint = true;
		}
		else
		{
			Player.Instance.IsOnSprayPaint = false;
		}
	}
}
