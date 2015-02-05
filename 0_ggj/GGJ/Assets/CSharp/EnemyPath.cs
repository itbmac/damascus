using UnityEngine;
using System.Collections;

public class EnemyPath : MonoBehaviour {
	public Vector3 pos1 = new Vector3(1, 1, 0);
	public Vector3 pos2 = new Vector3(-1, -1, 0);
	public float speed = 0.01f;
	bool dir = true;

	// Use this for initialization
	void Start () {
		gameObject.transform.position = pos1;
	}
	
	// Update is called once per frame
	void Update () {
		//Enemy moves from pos2 to pos1.
		if (dir) {
			//If the enemy hasn't yet hit the final position, keep moving.
			if(Vector3.Distance(gameObject.transform.position, pos1) > 0.1f){
				gameObject.transform.position += (pos1 - pos2) * speed;
			}
			//Otherwise, turn around.
			else{ 
				dir = false;
				gameObject.transform.position += (pos2 - pos1) * speed;
			}
		}
		//Enemy moves from pos1 to pos2.
		else{
			if(Vector3.Distance(gameObject.transform.position, pos2) > 0.1f){
				gameObject.transform.position += (pos2 - pos1) * speed;
			}
			else{ 
				dir = true;
				gameObject.transform.position += (pos1 - pos2) * speed;
			}
		}
	}
}
