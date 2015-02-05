using UnityEngine;
using System.Collections.Generic;

//needs background0-3 in the resources folder
//background0 is the looping bg music
[RequireComponent (typeof (AudioSource))]
public class Audio : MonoBehaviour 
{
	public AudioClip bgAudio;
	public List<AudioClip> audioList;
	public float timeSincePlay = 0.0f;
	public float timeToPlay = 8.0f;
	public bool playNext = true;
	
	void Start ()
	{
		audioList = new List<AudioClip>();
		audioList.Add((AudioClip)Resources.Load("background1"));
		audioList.Add((AudioClip)Resources.Load("background2"));
		bgAudio = (AudioClip)Resources.Load("LOOP");
		audio.clip = bgAudio;
		audio.loop = true;
		audio.Play();
	}
	
	void Update ()
	{

	}
	
}
