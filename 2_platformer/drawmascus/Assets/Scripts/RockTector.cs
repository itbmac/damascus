using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockTector : MonoBehaviour {

	public GameObject RockContainer;
	
	HashSet<GameObject> Rocks = new HashSet<GameObject>();
	
	bool activated = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!activated && Rocks.Count >= RockContainer.transform.childCount - 1) {
			activated = true;
			gameObject.layer = LayerMask.NameToLayer("BoulderGuide");
			collider2D.isTrigger = false;
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.transform.parent && coll.transform.parent.gameObject == RockContainer)
			Rocks.Add(coll.gameObject);
		
	}
}
