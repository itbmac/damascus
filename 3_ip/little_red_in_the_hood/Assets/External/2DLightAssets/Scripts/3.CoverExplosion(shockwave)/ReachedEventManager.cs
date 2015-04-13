using UnityEngine;
using System.Collections;

public class ReachedEventManager : MonoBehaviour {

	DynamicLight light2d;
	//GameObject myGO;
	GameObject[] GOsReached;
	TextMesh text;



	// Use this for initialization
	void Start () {
		light2d = GameObject.Find("2DLight").GetComponent<DynamicLight>() as DynamicLight;
		//myGO = GameObject.Find("hexagon");
		text = GameObject.Find("text").GetComponent<TextMesh>();

		// Add listener
		light2d.OnReachedGameObjects += waveReach;



	}




	void waveReach(GameObject[] g){

		bool found = false;
		string gsName = "";

		foreach(GameObject gs in g){
			if(gameObject.GetInstanceID() == gs.GetInstanceID()){
				found = true;
				gsName = gs.name;
			}
		}
		if(found == true){
			text.text = "PLAYER REACHED!!  _" + gsName +"__" + Time.time;
		}else{
			text.text = "in safe place";
		}

	}


}
