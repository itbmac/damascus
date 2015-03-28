using UnityEngine;
using System.Collections;

public class Traffic_Light : MonoBehaviour {
	public enum lightColor{NONE, GREEN, YELLOW, RED};
	public lightColor trafficLightColor;

	public Sprite graphicNone;
	public Sprite graphicGreen;
	public Sprite graphicYellow;
	public Sprite graphicRed;

	SpriteRenderer spriteRenderer;
	BoxCollider2D boxCollider;
	bool visited;

	// Use this for initialization
	void Start () {
		visited = false;
		spriteRenderer = this.GetComponent("SpriteRenderer") as SpriteRenderer;
		boxCollider = this.GetComponent("BoxCollider2D") as BoxCollider2D;
	}

	public void SetLightColor(lightColor newColor) {
		if (newColor != lightColor.NONE) {
			if (newColor == lightColor.GREEN) {
				spriteRenderer.sprite = graphicGreen;
				boxCollider.enabled = false;
			}
			else if (newColor == lightColor.YELLOW) {
				spriteRenderer.sprite = graphicYellow;
				boxCollider.enabled = false;
			}
			else if (newColor == lightColor.RED) {
				spriteRenderer.sprite = graphicRed;
				boxCollider.enabled = true;
			}

			trafficLightColor = newColor;
		}
	}

	public void SetColliderState(bool enabled) {
		boxCollider.enabled = enabled;
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (!visited) {
			visited = true;
		}
	}
}
