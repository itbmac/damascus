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
	
	IEnumerator DisableRocks() {
		yield return new WaitForSeconds(2);
	
		foreach (var g in Rocks) {
			g.rigidbody2D.isKinematic = true;
			g.collider2D.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!activated && Rocks.Count >= RockContainer.transform.childCount) {
			activated = true;
			gameObject.layer = LayerMask.NameToLayer("BoulderGuide");
			collider2D.isTrigger = false;
			
			StartCoroutine(DisableRocks());
		}
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.transform.parent && coll.transform.parent.gameObject == RockContainer)
			Rocks.Add(coll.gameObject);
		
	}
}
