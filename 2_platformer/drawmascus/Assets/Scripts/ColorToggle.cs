using UnityEngine;
using System.Collections;

public class ColorToggle : MonoBehaviour {
	SpriteRenderer sr;

	//Our wolf.
	public Player wolf;
	public SpriteRenderer wolfSr;

	public bool wolfInRange = false;

	//The color which will turn the drawing into a real object.
	public Color realColor = Color.red;

	//Current state of the object.
	public bool hasColor = true;
	
	//Sprites of the object when it's real or a drawing.
	public Sprite realSprite;
	public Sprite drawingSprite;
	
	public void giveColor(){
		Color c = wolf.currentColor;
		print("Giving color: " + c);

		//If the wolf does not have a color to give, return.
		if(c == Color.white) return;

		if(!hasColor){
		//If c wasn't the original color, layer color over the drawing.
			if(c != realColor){
				sr.color = c;
				sr.sprite = drawingSprite;
			}
			//If c was the original color, set the sprite to the real object.
			else{ 
				sr.color = Color.white;
				sr.sprite = realSprite;
			}

			hasColor = true;
			wolf.currentColor = Color.white;
			wolfSr.color = Color.white;
		}

		//If the object is currently colored, alert the user.
		else{
			//Play a sound? Animation?
			print("ERROR: Object currently colored.");
		}
	}
	
	public void takeColor(){
		Color c = wolf.currentColor;
		print("Taking color");

		//If the wolf already has a color, return.
		if(c != Color.white) return;

		if(hasColor){
			Color temp;

			if(sr.color == Color.white) temp = realColor;
			else temp = sr.color;

			wolf.currentColor = temp;
			wolfSr.color = temp;

			hasColor = false;
			sr.color = Color.white;
			sr.sprite = drawingSprite;
		}

		//If the object is an empty drawing currently, do nothing.
	}

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>(); 
		wolf = GameObject.FindWithTag("Wolf").GetComponent<Player>();
		wolfSr = GameObject.FindWithTag("Wolf").GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("ColorToggle")) {
			if(wolfInRange){
				//If the wolf is currently white, then it is trying to take a color.
				if(wolf.currentColor == Color.white){
					takeColor();
				}
				//If the wolf is currently colored, then it is trying to give a color.
				else{
					giveColor();
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		wolfInRange = true;
		print("Wolf in range.");
	}

	void OnTriggerExit2D(Collider2D other){
		wolfInRange = false;
		print("Wolf out of range.");
	}
}
