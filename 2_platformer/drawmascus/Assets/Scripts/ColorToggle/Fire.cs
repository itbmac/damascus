using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ColorToggle))]
public class Fire : MonoBehaviour {

	public Signal[] Real;
	
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void OnTurnReal() {
		foreach (var s in Real)
			s.Invoke();		

		anim.SetBool("IsDrawing", false);
	}
	
	public void OnTurnDrawing() {
		anim.SetBool("IsDrawing", true);
	}
}
