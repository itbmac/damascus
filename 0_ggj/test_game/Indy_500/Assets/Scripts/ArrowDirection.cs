using UnityEngine;
using System.Collections;

public class ArrowDirection : MonoBehaviour {

	public GameObject target;
	public GameObject car;
	Transform carPos;

	public float graphicAngleOffset = 90;
	public float graphicPositionOffset = 2.5f;

	public float angle;

	// Use this for initialization
	void Start () {
		carPos = car.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 A = target.transform.position - car.transform.position;
		Vector3 B = Vector3.up;
		//float angle = Vector3.Angle(targetDir, forward);
		//float angle = Vector3.Angle(target.transform.position, car.transform.position);

		angle = Vector3.Angle(A, B);
		//Vector3 cross = Vector3.Cross(A, B);
		if (car.transform.position.x < target.transform.position.x) {
			angle *= -1;
		}

		angle += graphicAngleOffset;

		//transform.rotation = new Quaternion(0, 0, angle* Mathf.Deg2Rad, 0);

		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		transform.position = carPos.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * graphicPositionOffset, Mathf.Sin(angle * Mathf.Deg2Rad) * graphicPositionOffset , 0);

		//transform.rotation = new Quaternion(angle*Mathf.Deg2Rad);
	}
}
