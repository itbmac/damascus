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
			agent.SetDestination(Player.Instance.transform.position);

			LastUpdateTime = Time.time;
		}
	}

	void OnDestinationInvalid() {
		if (LastUpdateTime + updateRate <= Time.time) {	
			Vector3 searchRadius = Random.insideUnitSphere * 12f;
			searchRadius.z = 0;
			agent.SetDestination(Player.Instance.transform.position + searchRadius);
			
			LastUpdateTime = Time.time;
		}
	}
	
//	void OnEnable(){
//		agent.OnDestinationReached += SetNewDestination;
//		agent.OnDestinationInvalid += OnDestinationInvalid;
//	}
//	
//	void OnDisable(){
//		agent.OnDestinationReached -= SetNewDestination;
//		agent.OnDestinationInvalid -= OnDestinationInvalid;
//	}
//	
//	void SetNewDestination() {
//		
//	}
}
