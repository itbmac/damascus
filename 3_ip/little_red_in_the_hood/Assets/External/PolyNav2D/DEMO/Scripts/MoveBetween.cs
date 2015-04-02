using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//example. moving between some points at random
[RequireComponent(typeof(PolyNavAgent))]
public class MoveBetween : MonoBehaviour{

	public List<Vector2> WPoints = new List<Vector2>();

	private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}
	
	public bool ViceCopMode = false;

	void OnEnable(){
		agent.OnDestinationReached += MoveRandom;
		agent.OnDestinationInvalid += MoveRandom;
	}

	void OnDisable(){
		agent.OnDestinationReached -= MoveRandom;
		agent.OnDestinationInvalid -= MoveRandom;
	}
	
	float nextChange;
	void Update() {
		if (!ViceCopMode) return;
		
		if (Time.time > nextChange)
			MoveRandom();	
	}

	IEnumerator Start(){
		yield return new WaitForSeconds(1);
		if (WPoints.Count > 0)
			MoveRandom();
	}

	void MoveRandom(){
		nextChange = Time.time + Random.Range(2.0F, 10.0F);	
		agent.SetDestination(WPoints[Random.Range(0, WPoints.Count)]);		
	}

	void OnDrawGizmosSelected(){
		for ( int i = 0; i < WPoints.Count; i++)
			Gizmos.DrawSphere(WPoints[i], 0.15f);			
	}
}
