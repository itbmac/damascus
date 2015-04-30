using UnityEngine;
using System.Collections;

public class MyMonoBehaviour : MonoBehaviour {

	// TODO; save these all to private variables lazily

	protected SpriteRenderer spriteRenderer {
		get {
			return GetComponent<SpriteRenderer>();
		}
	}
	
	protected Sprite sprite {
		get {
			return spriteRenderer.sprite;
		}
		
		set {
			spriteRenderer.sprite = value;
		}
	}
	
	protected Renderer renderer {
		get {
			return GetComponent<Renderer>();
		}
	}
	
	protected Color color {
		get {
			return spriteRenderer.color;
		}
		
		set {
			spriteRenderer.color = value;
		}
	}
	
	protected Animator anim {
		get {
			return GetComponent<Animator>();
		}
	}
	
	protected AudioSource audio {
		get {
			return GetComponent<AudioSource>();
		}
	}
	
	private Collider2D _collider2D;
	protected Collider2D collider2D {
		get {
			if (_collider2D == null) _collider2D = GetComponent<Collider2D>();
			return _collider2D;
		}
	}
	
	private Rigidbody2D _rigidBody2D;
	protected Rigidbody2D rigidBody2D {
		get {
			if (_rigidBody2D == null) _rigidBody2D = GetComponent<Rigidbody2D>();
			return _rigidBody2D;
		}
	}
}
