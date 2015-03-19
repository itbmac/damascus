using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ColorToggle : MonoBehaviour {
	SpriteRenderer sr;

	//Our wolf.
	Player wolf;

	//The color which will turn the drawing into a real object.
	public Color RealColor = Color.red;

	//Current color state of the object.
	bool hasColor = true;
	
	public bool StartAsDrawing = true;
	
	//Sprites of the object when it's real or a drawing.
	public Sprite RealSprite;
	public Sprite DrawingSprite;
	
	private void RefreshRigidbodies() {
		// Hack to cause any rigidbodies intersecting this static object
		// to wake up (i.e. see they are/are not colliding with me)
		collider2D.enabled = false;
		collider2D.enabled = true;
	}
	
	private void TurnReal() {
		gameObject.layer = LayerMask.NameToLayer("Real");
		RefreshRigidbodies();
		
		SendMessage("OnTurnReal");
	}
	
	private void TurnDrawing() {
		gameObject.layer = LayerMask.NameToLayer("Drawing");
		RefreshRigidbodies();
		
		SendMessage("OnTurnDrawing");
	}
	
	public void GiveColor(){
		Color wolfColor = wolf.CurrentColor;

		//If the wolf does not have a color to give, return.
		if(wolfColor == Color.white) return;

		if(!hasColor){
		//If c wasn't the original color, layer color over the drawing.
			if(wolfColor != RealColor){
				sr.color = wolfColor;
				sr.sprite = DrawingSprite;
				TurnDrawing();
			}
			//If c was the original color, set the sprite to the real object.
			else{ 
				sr.color = Color.white;
				sr.sprite = RealSprite;
				TurnReal();
			}

			hasColor = true;
			wolf.TransitionToColor(Color.white);
		}

		//If the object is currently colored, alert the user.
		else{
			//Play a sound? Animation?
			print("ERROR: Object currently colored.");
		}
	}
	
	public void TakeColor(){
		Color wolfColor = wolf.CurrentColor;

		//If the wolf already has a color, return.
		if(wolfColor != Color.white) return;

		if(hasColor){
			if(sr.color == Color.white) 
				wolf.TransitionToColor(RealColor);
			else 
				wolf.TransitionToColor(sr.color);

			hasColor = false;
			sr.color = Color.white;
			sr.sprite = DrawingSprite;
			TurnDrawing();
		}

		//If the object is an empty drawing currently, do nothing.
	}

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>(); 
		wolf = GameObject.FindWithTag("Wolf").GetComponent<Player>();
		
		sr.color = Color.white;
		if (StartAsDrawing) {
			sr.sprite = DrawingSprite;
			hasColor = false;
			TurnDrawing();
		} else {
			sr.sprite = RealSprite;		
			hasColor = true;
			TurnReal();
		}
	}
}
