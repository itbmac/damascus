using UnityEngine;
using System.Collections;

public class CrashPlayer : MonoBehaviour 
{
	public static CrashPlayer reference;
	public AudioClip crash;
	
	// Use this for initialization
	void Start ()
	{
		crash = (AudioClip)Resources.Load("crash");
		reference = this;
	}
	
	public void playCrash ()
	{
		AudioSource.PlayClipAtPoint(crash, new Vector3(0,0,0));
	}
}
