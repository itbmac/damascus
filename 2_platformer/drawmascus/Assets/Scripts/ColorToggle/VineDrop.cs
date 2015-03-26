using UnityEngine;
using System.Collections;

public class VineDrop : Colorable {

	public GameObject ItemToDrop;
	bool hasDroppedItem;	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnTurnReal() {
		if (!hasDroppedItem) {
			hasDroppedItem = true;
			Instantiate(ItemToDrop, transform.position, Quaternion.identity);
		}
	}
	
	public override void OnTurnDrawing() {		
		
	}
}
