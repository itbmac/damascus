using UnityEngine;
using System.Collections;

public class PixelTest : MyMonoBehaviour {

	public Texture2D originalTexture;

	public Texture2D myTexture;
	public Sprite mySprite;

	// Use this for initialization
	void Start () {
	
//		var original = spriteRenderer.sprite.texture;
		
		var t = Instantiate(originalTexture); //new Texture2D(original.width, original.height,  TextureFormat.RGB24, false);
		
//		Instantiate(spriteRenderer.sprite.texture);
		
		var pixels = t.GetPixels();
		
		for (int i = 0; i < t.height; i++) {
			if (i % 10 < 5) continue;
			for (int j = 0; j < t.width; j++) {
					pixels[i * t.width + j] = Color.black;
			}
		}
		
		t.SetPixels(pixels);
		t.Apply();
		print ("the deed is done");
		
		myTexture = t;
		
		GetComponent<Renderer>().material.mainTexture = myTexture;
		
		
//		mySprite = Sprite.Create(t, spriteRenderer.sprite.rect, spriteRenderer.sprite.pivot);
//		mySprite.name = "Foo";
	
//		spriteRenderer.sprite = mySprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
