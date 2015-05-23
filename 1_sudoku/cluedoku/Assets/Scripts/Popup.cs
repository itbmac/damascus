using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Popup : MonoBehaviour {

	public enum DismissBehavior {DoNothing, LoadNextScene, ReloadCurrentScene, RestartGame, TriggerOtherPopup, LoadNextBoard}
	
	public DismissBehavior OnDismiss;
	public GameObject OtherPopup;
	public AudioClip TriggerSound;
	public AudioClip DismissSound;
	public bool MustClickOnObject;
	public float MinTimeToLive = 0.1f;
	
	private bool triggered;
	private float triggerTime;
	
	public event Action Dismissed;
	
	public Sprite MySprite {
		set {
			var sr = (SpriteRenderer)GetComponent<Renderer>();
			sr.sprite = value;
		}
	}

	// Use this for initialization
	void Start () {
		var sr = (SpriteRenderer)GetComponent<Renderer>();
		triggered = false;
		sr.enabled = false;
		GetComponent<Collider2D>().enabled = false;
		
		if (sr.sortingLayerName != "Popup")
			Debug.LogWarning("Popup" + name + " must have renderer in sorting layer Popup -- and this must be fixed outside of play mode.");
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered && !MustClickOnObject && Input.anyKeyDown) {
			Dismiss();
		}
	}
	
	void OnMouseDown() {
		if (triggered)
			Dismiss();
	}
	
	public void Trigger() {		
		triggered = true;
		triggerTime = Time.time;
		((SpriteRenderer)GetComponent<Renderer>()).enabled = true;
		GetComponent<Collider2D>().enabled = true;
		GameManager.Instance.CurrentPopup = true;
		if (TriggerSound)
			GetComponent<AudioSource>().PlayOneShot(TriggerSound);
	}
	
	private IEnumerator YieldActionAfter(Action action, float seconds) {
		yield return new WaitForSeconds(seconds);
		action();
	}
	
	private void RunActionAfter(Action action, float seconds) {
		StartCoroutine(YieldActionAfter(action, seconds));
	}
	
	private void LoadLevelDelayed(int level, float seconds) {
		RunActionAfter(() => Application.LoadLevel(level), seconds);		
	}
	
	private void Dismiss() {
		if (Time.time - triggerTime < MinTimeToLive)
			return;
	
		triggered = false;
		((SpriteRenderer)GetComponent<Renderer>()).enabled = false;
		GetComponent<Collider2D>().enabled = false;
		GameManager.Instance.CurrentPopup = false;
		
		if (DismissSound)
			GetComponent<AudioSource>().PlayOneShot(DismissSound);
			
		if (Dismissed != null)
			Dismissed();
		
		if (OnDismiss == DismissBehavior.LoadNextScene)
			Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
		else if (OnDismiss == DismissBehavior.ReloadCurrentScene)
			Application.LoadLevel(Application.loadedLevel);
		else if (OnDismiss == DismissBehavior.RestartGame)
			Application.LoadLevel(0);
		else if (OnDismiss == DismissBehavior.TriggerOtherPopup)
			OtherPopup.SendMessage("Trigger");
		else if (OnDismiss == DismissBehavior.LoadNextBoard)
			BoardManager.Instance.NewBoard();
		
	}
}
