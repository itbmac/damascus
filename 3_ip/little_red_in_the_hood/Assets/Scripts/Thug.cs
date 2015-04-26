using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PolyNavAgent))]
public class Thug : MyMonoBehaviour {

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
	
	int WPointsIndex = 0;
	
	void OnEnable(){
		agent.OnDestinationReached += MoveRandom;
		agent.OnDestinationInvalid += MoveRandom;
	}
	
	void OnDisable(){
		agent.OnDestinationReached -= MoveRandom;
		agent.OnDestinationInvalid -= MoveRandom;
	}
	
	Vector2 startPos;
	
	void Update() {
	}
	
	void Start(){
		startPos = transform.position;
		if (WPoints.Count > 0)
			MoveRandom();
	}
	
	void MoveRandom() {		
		WPointsIndex = (WPointsIndex + 1) % WPoints.Count;
		
		agent.SetDestination(WPoints[WPointsIndex] + startPos);		
	}
	
	void OnDrawGizmosSelected(){
		var pos = Application.isPlaying ? startPos : (Vector2)transform.position;
		
		for ( int i = 0; i < WPoints.Count; i++) {
			int prev = i - 1;
			if (prev < 0)
				prev = WPoints.Count - 1;
			Gizmos.DrawLine(WPoints[prev] + pos, WPoints[i] + pos);
			Gizmos.DrawSphere(WPoints[i] + pos, 0.15f);			
		}
	}
}
