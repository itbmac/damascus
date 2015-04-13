using UnityEngine;
using System.Collections;

public class MyMonoBehaviour : MonoBehaviour {

	protected SpriteRenderer spriteRenderer {
		get {
			return GetComponent<SpriteRenderer>();
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
