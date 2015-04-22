using UnityEngine;
using System.Collections;

public class MyMonoBehaviour : MonoBehaviour {

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
}
