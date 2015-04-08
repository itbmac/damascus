using UnityEngine;
using System.Collections;

public class ExperimentSprite : MyMonoBehaviour {

	// Use this for initialization
	void Start () {	
		var texture = Instantiate(spriteRenderer.sprite.texture);
		
		var pixels = texture.GetPixels();
		
		for (int i = 0; i < texture.height; i++) {
			if (i % 10 < 5) continue;
			for (int j = 0; j < texture.width; j++) {
				pixels[i * texture.width + j] = Color.black;
			}
		}
		
		texture.SetPixels(pixels);
		texture.Apply();	
	
		spriteRenderer.sprite = Sprite.Create(
			texture,
			new Rect(0, 0, texture.width, texture.height),
			new Vector2(0.5f,0.5f)
		);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
