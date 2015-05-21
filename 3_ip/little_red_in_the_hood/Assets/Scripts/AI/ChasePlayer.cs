using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
public class ChasePlayer : MyMonoBehaviour {

	const float AngleOffset = 90;
	const float updateRate = 0.5f;
	float LastUpdateTime;

	private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get 
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}

	// Use this for initialization
	void Start () {
		LastUpdateTime = -updateRate;
	
	}
	
	// Update is called once per frame
	void Update () {
		var newAngle = agent.movingDirection.AngleInRadians() * Mathf.Rad2Deg + AngleOffset;
		
		var euler = transform.eulerAngles;
		euler.z = Mathf.LerpAngle(euler.z, newAngle, .05f);
		transform.eulerAngles = euler;	

		if (LastUpdateTime + updateRate <= Time.time) {	
			LastUpdateTime = Time.time;
			agent.SetDestination(Player.Instance.transform.position);
		}
	}

	void OnDestinationInvalid() {
		Debug.Log ("OnDestinationInvalid");
		if (LastUpdateTime + updateRate <= Time.time) {
			LastUpdateTime = Time.time;

			var searchRadius = Random.insideUnitCircle * 12f;
			agent.SetDestination((Vector2)Player.Instance.transform.position + searchRadius);
		}
	}
	
	void OnEnable(){
		agent.OnDestinationReached += OnDestinationReached;
		agent.OnDestinationInvalid += OnDestinationInvalid;
	}
	
	void OnDisable(){
		agent.OnDestinationReached -= OnDestinationReached;
		agent.OnDestinationInvalid -= OnDestinationInvalid;
	}
	
	void OnDestinationReached() {
		print ("Reached");
	}
}
