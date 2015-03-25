using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
public class Cactus : MonoBehaviour {

	GameObject Flower;
	bool flowerSpawned;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnTurnReal() {
		// spawn flower, if not already
		if (!flowerSpawned) {
			flowerSpawned = true;
			transform.FindChild("Flower").gameObject.SetActive(true);
			print ("swawning flower!");
		}
		
	}
	
	public void OnTurnDrawing() {
		
	}
}
