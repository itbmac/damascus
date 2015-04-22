using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
public class ChasePlayer : MyMonoBehaviour {

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
		
	
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetDestination(Player.Instance.transform.position);
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
