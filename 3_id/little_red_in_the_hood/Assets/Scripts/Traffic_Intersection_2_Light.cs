using UnityEngine;
using System.Collections;

public class Traffic_Intersection_2_Light : MonoBehaviour {

	public GameObject light1;
	public GameObject light3;

	public Traffic_Light.lightColor colorPos1n3;

	public int intervalGreen = 200;
	public int intervalYellow = 25;
	int intervalRed = 225;
	int frameNumber = 0;

	bool startSet = false;

	Traffic_Light L1;
	Traffic_Light L3;

	// Use this for initialization
	void Start () {
		L1 = light1.GetComponent("Traffic_Light") as Traffic_Light;
		L3 = light3.GetComponent("Traffic_Light") as Traffic_Light;

		intervalRed = intervalGreen + intervalYellow;
		frameNumber = intervalRed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!startSet) {
			L1.SetLightColor(colorPos1n3);
			L3.SetLightColor(colorPos1n3);
			startSet = true;
		}

		if (frameNumber == 0) {
			frameNumber = intervalRed;

			AdvanceLightColor(L1);
			AdvanceLightColor(L3);
		}
		else {
			frameNumber--;

			if (frameNumber == intervalYellow) {
				AdvanceLightColor(L1, true);
				AdvanceLightColor(L3, true);
			}
		}
	}

	void AdvanceLightColor(Traffic_Light TL, bool yellowIntervalChange = false) {
		if (!yellowIntervalChange) {
			if (TL.trafficLightColor == Traffic_Light.lightColor.RED) {
				TL.SetLightColor(Traffic_Light.lightColor.GREEN);
			}
			else if (TL.trafficLightColor == Traffic_Light.lightColor.YELLOW) {
				TL.SetLightColor(Traffic_Light.lightColor.RED);
			}
			else if (TL.trafficLightColor == Traffic_Light.lightColor.GREEN) {
				TL.SetLightColor(Traffic_Light.lightColor.YELLOW);
			}
		}
		else if (TL.trafficLightColor == Traffic_Light.lightColor.GREEN) {
			TL.SetLightColor(Traffic_Light.lightColor.YELLOW);
		}
	}

//	void OnTriggerStay2D(Collider2D other) {
//		if (other.tag == "Car") {
//			L1.SetColliderState(false);
//			L3.SetColliderState(false);
//		}
//	}
//
//	void OnTriggerExit2D(Collider2D other) {
//		if (other.tag == "Car") {
//			if (L1.trafficLightColor == Traffic_Light.lightColor.RED) {
//				L1.SetColliderState(true);
//			}
//
//			if (L3.trafficLightColor == Traffic_Light.lightColor.RED) {
//				L3.SetColliderState(true);
//			}
//		}
//	}
}
