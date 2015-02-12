using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Submit : MonoBehaviour {

	public Sprite spriteDisabled;
	public Sprite spriteEnabled;
	public AudioClip ClickedWhenDisabled;
	
	private bool submitEnabled;
	
	void Update() {
		SpriteRenderer spriteRenderer = (SpriteRenderer)renderer;
		
		submitEnabled = BoardManager.Instance.IsCurrentBoardFilled();
		if (submitEnabled)
			spriteRenderer.sprite = spriteEnabled;
		else
			spriteRenderer.sprite = spriteDisabled;
	}

	void OnMouseDown() {
		if (submitEnabled)
			BoardManager.Instance.Submit();
		else if (ClickedWhenDisabled)
			audio.PlayOneShot(ClickedWhenDisabled);
	}
}
