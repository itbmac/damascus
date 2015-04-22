using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DistanceMeasure : MonoBehaviour {

	public float Distance;

#if UNITY_EDITOR
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Distance = Vector2.Distance(transform.GetChild(0).position, transform.GetChild(1).position);
	}
#endif
}
