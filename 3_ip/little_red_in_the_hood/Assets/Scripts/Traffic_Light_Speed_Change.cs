//using UnityEngine;
//using System.Collections;
//
//public class Traffic_Light_Speed_Change : MonoBehaviour {
//	public enum lightColor{NONE, GREEN, YELLOW, RED};
//	public lightColor trafficLightColor;
//
//	public Sprite graphicNone;
//	public Sprite graphicGreen;
//	public Sprite graphicYellow;
//	public Sprite graphicRed;
//
//	bool visited;
//
//	// Use this for initialization
//	void Start () {
//		visited = false;
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//	
//	void OnTriggerExit2D(Collider2D other) {
//		if (!visited) {
//			IndyControls cont = other.attachedRigidbody.transform.gameObject.GetComponent("IndyControls") as IndyControls; 
//			cont.currentLightPassed = trafficLightColor;
//			cont.TrafficLightSpeedChangeApplied = false;
//			visited = true;
//		}
//	}
//}
