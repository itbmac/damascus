using UnityEngine;
using System.Collections;

public class LapTracker : MonoBehaviour {

	public GameObject lineStartEnd;
	public GameObject line1;

	public int NumLapsCompleted = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckLines();

		GetComponent<GUIText>().text = NumLapsCompleted + " Laps";
	}

	void CheckLines () {
		GameObject[] trackLines = GameObject.FindGameObjectsWithTag("TrackLine");

		foreach (GameObject line in trackLines) {
			TrackLinePassed lineChecker = line.GetComponent("TrackLinePassed") as TrackLinePassed;
			
			if (lineChecker && lineChecker.linePassed) {
			}
			else {
				return;
			}
		}

		foreach (GameObject line in trackLines) {
			TrackLinePassed lineChecker = line.GetComponent("TrackLinePassed") as TrackLinePassed;
			
			if (lineChecker) {
				lineChecker.linePassed = false;
			}
		}

		NumLapsCompleted++;
	}
}
