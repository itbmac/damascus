using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[System.NonSerialized]
	public bool CurrentPopup = false;
	
	public static GameManager Instance;

	void Awake () {
		Instance = this;
	}
}
