using UnityEngine;
using System.Collections;

public class Roomba : MonoBehaviour {
	public bool objectCollided = false;
	public float move = .1f;
	Vector3 lastPos;
	Vector3 lastLastPos;
	public bool magnetPinging = false;
	public bool pinging = false;

	IEnumerator magnetize(Transform end){
		objectCollided = false;

		Vector3 start = gameObject.transform.position;
		Vector3 offset = gameObject.transform.position - end.position;
		float pos = 0.0f;
		
		while (pos < 1.0f) {
			pos += Time.deltaTime * 0.5f; 
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, end.position, pos);

			if(objectCollided){ 
				print("pos: " + pos);
				gameObject.transform.position = lastLastPos;
				lastPos = lastLastPos;
				break;
			}
	
			if (Vector3.Distance(gameObject.transform.position, end.position) < 0.1f)
			{
				break;
			}
			yield return null;
		}

		magnetPinging = true;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	void Update(){
		lastLastPos = lastPos;
		lastPos = gameObject.transform.position;

		//Also check for how far the object is!
		if (Input.GetMouseButtonDown(0)){ // if left button pressed...
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hits = Physics.RaycastAll(ray);
			
			foreach (RaycastHit hit in hits) 
			{
				if (hit.transform.gameObject.name == "Magnet")
				{
					StartCoroutine(magnetize(hit.transform));
					break;
				}
			}
		}

		else{
			if (Input.GetKey (KeyCode.W)) {
				Vector3 newPos = gameObject.transform.position;
				newPos.y += move;
				gameObject.transform.position = newPos;
			}
			if ( Input.GetKey(KeyCode.S) ){
				Vector3 newPos = gameObject.transform.position;
				newPos.y -= move;
				gameObject.transform.position = newPos;
			}
			if ( Input.GetKey(KeyCode.D) ){
				Vector3 newPos = gameObject.transform.position;
				newPos.x += move;
				gameObject.transform.position = newPos;
			}
			if ( Input.GetKey(KeyCode.A) ){
				Vector3 newPos = gameObject.transform.position;
				newPos.x -= move;
				gameObject.transform.position = newPos;
			}
			if (Input.GetKey (KeyCode.Space)){
				EchoLocation.reference.ping(true);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.name == "Wall"){
			objectCollided = true;
			gameObject.transform.position = lastLastPos;
			lastPos = lastLastPos;
		}
		else if(other.gameObject.name == "Magnet"){
			magnetPinging = true;
		}
	}

	void OnTriggerStay(Collider other) {
		if(other.gameObject.name == "Wall"){
			objectCollided = true;
			gameObject.transform.position = lastLastPos;
			lastPos = lastLastPos;
		}
		else
		{
			objectCollided = false;
		}
	}


}
