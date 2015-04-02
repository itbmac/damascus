using UnityEngine;
using System.Collections;

public class Police : MonoBehaviour {

	public AudioClip GotPlayer	;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
//	bool isPlayerVisible() {
//		Vector2 dir = GetComponent<PolyNavAgent>().movingDirection;
//		
//		Vector2 playerPos = player.transform.position;
//		Vector2 toPlayer = playerPos - transform.position;
//		
//		float playerAngle = Vector3.Angle(toPlayer, -Vector2.right);
//		if (Mathf.Abs(playerAngle) > 45f) {
//			return false;
//		}
//		
//		if (toPlayer.magnitude > )
//		
//		bool playerVisible = Physics2D.Linecast
//			
//			Debug.DrawRay(transform.position, vel*25, Color.green);
//	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnDetectPlayer() {
		GetComponent<AudioSource>().PlayOneShot(GotPlayer);
		TheGameManager.Instance.Detected();
	}
}
