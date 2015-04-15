using UnityEngine;
using System.Collections;

public class MyMonoBehaviour : MonoBehaviour {

	protected SpriteRenderer spriteRenderer {
		get {
			return GetComponent<SpriteRenderer>();
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
