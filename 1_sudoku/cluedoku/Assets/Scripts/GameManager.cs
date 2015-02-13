using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Popup CurrentPopup;
	
	public static GameManager Instance;

	void Awake () {
		Instance = this;
	}
}
