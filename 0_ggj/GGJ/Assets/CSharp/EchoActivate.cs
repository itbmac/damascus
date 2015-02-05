using UnityEngine;
using System.Collections;

public class EchoActivate : MonoBehaviour {
	public Roomba parentScript;
	public EchoLocation echo;

	// Use this for initialization
	void Start () {
		echo = GetComponent<EchoLocation>();
		parentScript = gameObject.transform.parent.GetComponent<Roomba>();
	}
	
	// Update is called once per frame
	void Update () {
		if (parentScript.pinging) {
			echo.ping(true);
			parentScript.pinging = false;
		}
		else if (parentScript.magnetPinging)
		{
			echo.ping(false);
			parentScript.magnetPinging = false;
		}
	}
}
