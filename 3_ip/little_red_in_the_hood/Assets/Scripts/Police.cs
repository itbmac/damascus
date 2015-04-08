﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//example. moving between some points at random
[RequireComponent(typeof(PolyNavAgent))]
public class Police : MonoBehaviour {

	public AudioClip GotPlayer;
	public float InvestigativeSpeedMultiplier = 2.0f;
	const float CommunicationRange = 35.0f;
	
	public enum State {Normal, PlayerDetected, PlayerVisible}
	
	private State _currentState;
	public State CurrentState {
		get {
			return _currentState;
		}
		
		set {
			if (value == _currentState)
				return;
		
			if (value == State.Normal) {
				agent.maxSpeed = maxSpeed;
				MoveRandom();
			} else {
				TheGameManager.Instance.Detected();
				
				agent.maxSpeed = maxSpeed * InvestigativeSpeedMultiplier;
				
				if (_currentState == State.Normal)
					GetComponent<AudioSource>().PlayOneShot(GotPlayer);
			}
		
			_currentState = value;
		}
	}
	
	
	GameObject player;
	
	public Vector2[] WPoints;
	public bool ViceCopMode;
	
	Collider2D visionCone;
	
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
	
	Vector2 playerLastSeenPos;
	float playerLastSeenTime;
	
	const float PlayerDetectedTime = 10.0f;
	const float DetectedInvestigativeRadius = 1.5f;
	
	void OnEnable(){
		agent.OnDestinationReached += MoveRandom;
		agent.OnDestinationInvalid += OnDestinationInvalid;
	}
	
	void OnDisable(){
		agent.OnDestinationReached -= MoveRandom;
		agent.OnDestinationInvalid -= OnDestinationInvalid;
	}
	
	float nextChange;
	void Update() {	
		if (CurrentState == State.Normal) {
			if (ViceCopMode && Time.time > nextChange)
				MoveRandom();	
		} else {
			bool playerVisible = visionCone.IsTouching(player.GetComponent<Collider2D>());
			if (CurrentState == State.PlayerVisible) {
				playerLastSeenTime = Time.time;
				playerLastSeenPos = player.transform.position;
				agent.SetDestination(playerLastSeenPos);
			
				if (playerVisible) {
					CommunicatePlayerPos();
				} else
					CurrentState = State.PlayerDetected;
			} else if (CurrentState == State.PlayerDetected) {
				if (Time.time - playerLastSeenTime > PlayerDetectedTime)
					CurrentState = State.Normal;
			}
		}
	}
	
	float maxSpeed;
	IEnumerator Start(){
		player = FindObjectOfType<Player>().gameObject;
		visionCone = transform.Find("VisionCone").GetComponent<Collider2D>();
		maxSpeed = agent.maxSpeed;
		
		yield return new WaitForSeconds(1);
		if (WPoints.Length > 0)
			MoveRandom();
	}
	
	void MoveRandom() {
		if (CurrentState == State.Normal) {	
			if (ViceCopMode) {
				WPointsIndex = Random.Range(0, WPoints.Length);
				nextChange = Time.time + Random.Range(2.0F, 10.0F);
			} else {		
				WPointsIndex = (WPointsIndex + 1) % WPoints.Length;	
			}
			
			agent.SetDestination(WPoints[WPointsIndex]);
		} else if (CurrentState == State.PlayerDetected) {			
			int maxIterations = 5;
			Vector2 potentialPos;
			do {
				potentialPos = Random.insideUnitCircle * DetectedInvestigativeRadius;
			} while (--maxIterations > 0 && Physics2D.OverlapPoint(potentialPos, LayerMask.GetMask("Obstacle")));
		
			agent.SetDestination(potentialPos);		
		}		
	}
	
	IEnumerator MoveRandomSoon() {
		yield return new WaitForSeconds(1);
		MoveRandom();
	}
	
	void OnDestinationInvalid() {
		StartCoroutine(MoveRandomSoon());
	}
	
	void OnDrawGizmosSelected(){
		for ( int i = 0; i < WPoints.Length; i++)
			Gizmos.DrawSphere(WPoints[i], 0.15f);			
	}
	
//	bool isPlayerVisible() {
////		Vector2 dir = GetComponent<PolyNavAgent>().movingDirection;
////		
////		Vector2 playerPos = player.transform.position;
//////		Vector2 toPlayer = playerPos - transform.position;
//////		
//////		float playerAngle = Vector3.Angle(toPlayer, -Vector2.right);
//////		if (Mathf.Abs(playerAngle) > 45f) {
//////			return false;
//////		}
//////		
//////		if (toPlayer.magnitude > )
////		
////		int layerMask = LayerMask.GetMask("Player", "Default");
////		print (layerMask);
////		
////		bool playerVisible = Physics2D.Linecast(transform.position, player.transform.position, layerMask);
//		
//	}
	
	float nextCommunicate;
	void CommunicatePlayerPos() {	
		if (Time.time > nextCommunicate)
			nextCommunicate = Time.time + 2.0f;
		else
			return;
			
		foreach (Transform got in transform.parent) {
			var go = got.gameObject;
			float dist = Vector2.Distance(transform.position, go.transform.position);			
			if (go != gameObject && dist < CommunicationRange) {
				print ("Notifying to " + go.name);
				var police = go.GetComponent<Police>();
				police.NotifyPlayerPos(player.transform.position);
			}
		}
	}
	
	public void NotifyPlayerPos(Vector2 pos) {
		if (CurrentState != State.PlayerVisible) {
			playerLastSeenPos = pos;
			playerLastSeenTime = Time.time;
			CurrentState = State.PlayerDetected;
		}
	}
	
	void OnDetectPlayer() {
		if (CurrentState != State.Normal)
			return;
	
		CurrentState = State.PlayerVisible;
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
		
		}		
	}
}
