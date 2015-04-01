using UnityEngine;
using System.Collections;

public class Traffic_Intersection_2_Light_Corner : MonoBehaviour {

	public GameObject light1;
	public GameObject light2;

	public Traffic_Light.lightColor colorPos1;
	public Traffic_Light.lightColor colorPos2;

	public int intervalGreen = 200;
	public int intervalYellow = 25;
	int intervalRed = 225;
	int frameNumber = 0;

	bool startSet = false;

	Traffic_Light L1;
	Traffic_Light L2;

	// Use this for initialization
	void Start () {
		L1 = light1.GetComponent("Traffic_Light") as Traffic_Light;
		L2 = light2.GetComponent("Traffic_Light") as Traffic_Light;

		intervalRed = intervalGreen + intervalYellow;
		frameNumber = intervalRed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!startSet) {
			L1.SetLightColor(colorPos1);
			L2.SetLightColor(colorPos2);
			startSet = true;
		}

		if (frameNumber == 0) {
			frameNumber = intervalRed;

			AdvanceLightColor(L1);
			AdvanceLightColor(L2);
		}
		else {
			frameNumber--;

			if (frameNumber == intervalYellow) {
				AdvanceLightColor(L1, true);
				AdvanceLightColor(L2, true);
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
}
