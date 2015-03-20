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
	
	public bool RegeneratePolygonCollider = true;
	public bool LockAfterChange = false;
	
	private bool locked = false;
	
	private void RefreshPhysics() {
		// Hack to cause any rigidbodies intersecting this static object
		// to wake up (i.e. see they are/are not colliding with me)
		collider2D.enabled = false;
		collider2D.enabled = true;
		var polygonCollider = GetComponent<PolygonCollider2D>() ;
		if (polygonCollider && RegeneratePolygonCollider) {
			Destroy(polygonCollider);
			gameObject.AddComponent<PolygonCollider2D>();
		}
	}
	
	private void TurnReal() {
		gameObject.layer = LayerMask.NameToLayer("Real");
		RefreshPhysics();
		
		SendMessage("OnTurnReal");
	}
	
	private void TurnDrawing() {
		gameObject.layer = LayerMask.NameToLayer("Drawing");
		RefreshPhysics();
		
		SendMessage("OnTurnDrawing");
	}
	
	public bool GiveColor(){
		if (locked)
			return false;
	
		Color wolfColor = wolf.CurrentColor;

		//If the wolf does not have a color to give, return.
		if(wolfColor == Color.white) {
			print ("Wolf does not have a color to give");
			return false;
		}

		if(!hasColor){
		//If c wasn't the original color, reject
			if(wolfColor != RealColor){
				print("Wolf cannot give color to " + name + "; incompatible"); 
				return false;
				
//				sr.color = wolfColor;
//				sr.sprite = DrawingSprite;
//				TurnDrawing();
			}
			//If c was the original color, set the sprite to the real object.
			else{ 
				sr.color = Color.white;
				sr.sprite = RealSprite;
				TurnReal();
				locked = LockAfterChange;
			}

			hasColor = true;
			wolf.TransitionToColor(Color.white);
			return true;
		}

		//If the object is currently colored, alert the user.
		else{
			//Play a sound? Animation?
			print("ERROR: Object currently colored.");
			return false;
		}
	}
	
	public bool TakeColor(){
		if (locked)
			return false;
			
		Color wolfColor = wolf.CurrentColor;

		//If the wolf already has a color, return.
		if(wolfColor != Color.white) {
			print ("Wolf already has color");
			return false;
		}

		if(hasColor){
			if(sr.color == Color.white) 
				wolf.TransitionToColor(RealColor);
			else 
				wolf.TransitionToColor(sr.color);

			hasColor = false;
			sr.color = Color.white;
			sr.sprite = DrawingSprite;
			TurnDrawing();
			locked = LockAfterChange;
			
			return true;
		}

		print ("Wolf cannot take color from object without color - " + name);
		//If the object is an empty drawing currently, do nothing.
		return false;
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
