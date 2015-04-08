using UnityEngine;
using System.Collections;

public class ExperimentSprite : MyMonoBehaviour {

	public float UpdateTime = 1.0f;

	Texture2D originalTexture;
	Texture2D texture ;

	// Use this for initialization
	void Start () {	
		originalTexture = spriteRenderer.sprite.texture;
		texture = Instantiate(spriteRenderer.sprite.texture);
		spriteRenderer.sprite = Sprite.Create(
			texture,
			new Rect(0, 0, texture.width, texture.height),
			new Vector2(0.5f,0.5f)
		);
	}
	
	float nextUpdate;
	int min;
	
	// Update is called once per frame
	void Update () {
		if (Time.time < nextUpdate)
			return;
		else
			nextUpdate = Time.time + UpdateTime;
	
		var pixels = originalTexture.GetPixels();
		
		
		int max = min + 10;
		
		for (int i = 0; i < texture.height; i++) {
			int m = i % 50;
			if (!(min <= m && m <= max)) continue;
			
			for (int j = 0; j < texture.width; j++) {
				pixels[i * texture.width + j] = Color.black;
			}
		}
		
		texture.SetPixels(pixels);
		texture.Apply();
		
		min = (min + 10) % 50;
	}
}
