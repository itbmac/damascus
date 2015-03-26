using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
public class Fire : MonoBehaviour {

	public Signal[] Real;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnTurnReal() {
		foreach (var s in Real)
			s.Invoke();
	}
	
	public void OnTurnDrawing() {
		
	}
}
