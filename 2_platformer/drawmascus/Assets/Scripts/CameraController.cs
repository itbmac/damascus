using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	GameObject Target;
	public float DampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	
	Vector3 GetDestination() {		
		Transform target = Target.transform;
		Vector3 t_pos = target.position;
		//t_pos.y += 0.5f * scale;
		Vector3 point = camera.WorldToViewportPoint(t_pos);
		Vector3 delta = t_pos - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
		Vector3 destination = transform.position + delta;
		
		return destination;
	}

	// Use this for initialization
	void Start () {
		Target = FindObjectOfType<Player>().gameObject;
		transform.position = GetDestination();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.SmoothDamp(transform.position, GetDestination(), ref velocity, DampTime);
	}
}
