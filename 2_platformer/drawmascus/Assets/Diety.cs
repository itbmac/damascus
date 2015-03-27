using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Diety : MonoBehaviour {

	ColorToggle[] childs;
	
	bool nextLevel;

	// Use this for initialization
	void Start () {
		childs = GetComponentsInChildren<ColorToggle>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!nextLevel && childs.All(x => x.hasColor)) {
			Application.LoadLevel(Application.loadedLevel + 1);
			nextLevel = true;
		}
	
	}
}
