using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedSprite : MonoBehaviour {

	public List<Texture2D> frames = new List<Texture2D>();
	public float framesPerSecond = 10;
	public float spriteWidth = 410.0f;
	public float spriteHeight = 410.0f;

	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
	}

	void Update() {
		float index = (Time.time * framesPerSecond) % frames.Count;
		spriteRenderer.sprite = Sprite.Create(frames[(int) index], new Rect(0,0,spriteWidth,spriteHeight), new Vector2(0.5f, 0.5f));
	}
}
