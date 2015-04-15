using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//example. moving between some points at random
[RequireComponent(typeof(PolyNavAgent))]
public class Police : MyMonoBehaviour {

	public AudioClip GotPlayer;
	public float InvestigativeSpeedMultiplier = 2.0f;
	const float CommunicationRange = 0.0f;
	public bool DebugMode;
	public bool DogMode;
	
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
				agent.maxSpeed = maxSpeed * InvestigativeSpeedMultiplier;

				float dist = Vector2.Distance(transform.position, player.transform.position); 
				
				if ((dist < DetectionRange) && _currentState == State.Normal && value == State.PlayerVisible) {
					GetComponent<AudioSource>().PlayOneShot(GotPlayer);
					TheGameManager.Instance.Detected();
				}
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
	float playerFirstSeenTime;

	const float PlayerSeenTimeBeforeArrest = 1.5f;
	const float PlayerDetectedTime = 5.0f;
	const float DetectedInvestigativeRadius = 1.5f;
	const float DetectionRange = 7.0f;
	
	void OnEnable(){
		agent.OnDestinationReached += MoveRandom;
		agent.OnDestinationInvalid += OnDestinationInvalid;
	}
	
	void OnDisable(){
		agent.OnDestinationReached -= MoveRandom;
		agent.OnDestinationInvalid -= OnDestinationInvalid;
	}
	
	const float HealthToDrain = 1.0f;
	
	float nextChange;
	void Update() {	
	
		bool playerVisible = isPlayerVisible();
		
		if (playerVisible) {
			player.GetComponent<Player>().Health -= HealthToDrain * Time.deltaTime;
		}
		
		if (CurrentState == State.Normal) {
			if (playerVisible) {
				CurrentState = State.PlayerVisible;
			} else if (ViceCopMode && Time.time > nextChange)
				MoveRandom();	
		} else {			
			if (CurrentState == State.PlayerVisible) {
				playerLastSeenTime = Time.time;
				playerFirstSeenTime = Time.time;
				playerLastSeenPos = player.transform.position;
				agent.SetDestination(playerLastSeenPos);
			
				if (playerVisible) {
					CommunicatePlayerPos();
				} else
					CurrentState = State.PlayerDetected;
			} else if (CurrentState == State.PlayerDetected) {
				if (Time.time - playerLastSeenTime > PlayerDetectedTime)
					CurrentState = State.Normal;
				else if (playerVisible) {
					playerLastSeenTime = Time.time;
					TheGameManager.Instance.Detected();

//					if (playerLastSeenTime - playerFirstSeenTime > PlayerSeenTimeBeforeArrest) {
//						Application.LoadLevel(Application.loadedLevel);
//					}
				}
				else {
					//playerFirstSeenTime = Time.time;
				}
			}
		}
	}
	
	float maxSpeed;
	void Start(){
		player = FindObjectOfType<Player>().gameObject;		
		visionCone = transform.GetChild(0).GetComponent<Collider2D>();
		maxSpeed = agent.maxSpeed;
		
		if (WPoints.Length > 0) {
			Vector2 pos = new Vector2(transform.position.x, transform.position.y);

			for (int i = 0; i < WPoints.Length; i++) {
				WPoints[i] += pos;
			}

			MoveRandom();
		}
	}
	
	void MoveRandom() {
		if (CurrentState == State.Normal) {	
			if (ViceCopMode) {
				//WPointsIndex = Random.Range(0, WPoints.Length);
				WPointsIndex = (WPointsIndex + 1) % WPoints.Length;	
				nextChange = Time.time + Random.Range(2.0F, 10.0F);
			} else {		
				WPointsIndex = (WPointsIndex + 1) % WPoints.Length;	
			}
			
			agent.SetDestination(WPoints[WPointsIndex]);
		} else if (CurrentState == State.PlayerDetected) {			
			int maxIterations = 5;
			Vector2 potentialPos;
			do {
				potentialPos = playerLastSeenPos + Random.insideUnitCircle * DetectedInvestigativeRadius;
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
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);

		for ( int i = 0; i < WPoints.Length; i++)
			Gizmos.DrawSphere(WPoints[i] + pos, 0.15f);			
	}
	
	bool isPlayerVisible() {
		if (Player.Instance.StealthMode)
			return false;

		float dist = Vector2.Distance(transform.position, player.transform.position); 

		if (dist > DetectionRange)
			return false;

		if (Player.Instance.IsOnSprayPaint)
			return false;
	
		int layerMask = LayerMask.GetMask("Obstacle");		
		if (Physics2D.Linecast(transform.position, player.transform.position, layerMask)) {
			return false;
		}
			
		if (DogMode)
			return true;
		
		if (Player.Instance.IsUnderStreetlight && Vector2.Angle(-transform.up, player.transform.position - transform.position) < 45f) {
			return true;
		}
		
		return visionCone.IsTouching(player.GetComponent<Collider2D>());
	}
	
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
				Debug.DrawLine(transform.position, go.transform.position, Color.green, 0.5f);
				var police = go.GetComponent<Police>();
				police.NotifyPlayerPos(player.transform.position);
			}
		}
	}
	
	public void NotifyPlayerPos(Vector2 pos) {
		NotifyPlayerPos(pos, Time.time);
	}
	
	public void NotifyPlayerPos(Vector2 pos, float lastSeenTime) {
		if (CurrentState != State.PlayerVisible && lastSeenTime > playerLastSeenTime) {
			playerLastSeenPos = pos;
			playerLastSeenTime = lastSeenTime;
			CurrentState = State.PlayerDetected;
		}
	}
}
