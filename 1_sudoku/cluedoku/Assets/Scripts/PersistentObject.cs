using UnityEngine;
using System.Collections;

public class PersistentObject : MonoBehaviour {

//	private static PersistentObject Instance;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
		
//		if (Instance && Instance != this)
//			Destroy(gameObject);
//		else
//			Instance = this;
	}
}
