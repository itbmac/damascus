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
	
	new protected Renderer renderer {
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
	
	new protected AudioSource audio {
		get {
			return GetComponent<AudioSource>();
		}
	}
	
	private Collider2D _collider2D;
	new protected Collider2D collider2D {
		get {
			if (_collider2D == null) _collider2D = GetComponent<Collider2D>();
			return _collider2D;
		}
	}
}
