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

	// Use this for initialization
	void Start () {
		triggered = false;
		((SpriteRenderer)renderer).enabled = false;
		collider2D.enabled = false;
		
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
		((SpriteRenderer)renderer).enabled = true;
		collider2D.enabled = true;
		GameManager.Instance.CurrentPopup = this;
		if (TriggerSound)
			audio.PlayOneShot(TriggerSound);
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
		((SpriteRenderer)renderer).enabled = false;
		collider2D.enabled = false;
		GameManager.Instance.CurrentPopup = null;
		
		if (DismissSound)
			audio.PlayOneShot(DismissSound);
		
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
