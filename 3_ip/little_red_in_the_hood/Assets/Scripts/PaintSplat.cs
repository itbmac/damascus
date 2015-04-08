using UnityEngine;
using System.Collections;

public class PaintSplat : MonoBehaviour {

	public int TimeToLive = 4;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	bool added;
	
	IEnumerator Die() {
		yield return new WaitForSeconds(TimeToLive);
		if (added)
			GameObject.Find("Player").SendMessage("RemoveHidden");
			
		Destroy(gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") {
			other.SendMessage("AddHidden");
			added = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Player") {
			other.SendMessage("RemoveHidden");
			added = false;
		}
	}
}
