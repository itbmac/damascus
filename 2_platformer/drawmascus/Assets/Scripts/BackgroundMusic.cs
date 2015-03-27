using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	private static BackgroundMusic _instance;
	public bool Persist = true;
	
	void Awake() 
	{
		if (!Persist)
			return;
			
		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
			
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
				Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
