using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public bool CurrentPopup;
	
	public static GameManager Instance;

	void Awake () {
		Instance = this;
	}
}
