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
	public bool UseCollider = true;
	
	private bool locked = false;
	
	private void RefreshPhysics() {
		var polygonCollider = GetComponent<PolygonCollider2D>() ;
		if (polygonCollider && RegeneratePolygonCollider) {
			Destroy(polygonCollider);
			var col = gameObject.AddComponent<PolygonCollider2D>();
			
			col.enabled = UseCollider || hasColor;
		} else {
			// Hack to cause any rigidbodies intersecting this static object
			// to wake up (i.e. see they are/are not colliding with me)
			collider2D.enabled = false;
			collider2D.enabled = UseCollider || hasColor;
		}		
	}
	
	private void TurnReal() {
		Deselect();
		sr.sprite = RealSprite;		
		hasColor = true;
		gameObject.layer = LayerMask.NameToLayer("Real");
		RefreshPhysics();
		
		SendMessage("OnTurnReal", SendMessageOptions.DontRequireReceiver);
	}
	
	private void TurnDrawing() {
		Deselect();
		
		sr.sprite = DrawingSprite;
		hasColor = false;
		gameObject.layer = LayerMask.NameToLayer("Drawing");
		RefreshPhysics();
		
		SendMessage("OnTurnDrawing", SendMessageOptions.DontRequireReceiver);
	}
	
	public bool CanGiveColor(bool printReason = false) {
		if (locked)
			return false;
		
		Color wolfColor = wolf.CurrentColor;
		
		//If the wolf does not have a color to give, return.
		if(wolfColor == Color.white) {
			if (printReason)
				print ("Wolf does not have a color to give");
			return false;
		}
		
		//If the object is currently colored, alert the user.
		if (hasColor)
		{
			//Play a sound? Animation?
			if (printReason)
				print("ERROR: Object currently colored.");
			return false;
		}
		
		//If c wasn't the original color, reject
		if(wolfColor != RealColor){
			if (printReason)
				print("Wolf cannot give color to " + name + "; incompatible"); 
			return false;
		}
		
		return true;
	}
	
	public bool GiveColor(){
		if (!CanGiveColor(true))
			return false;		
		
		//If c was the original color, set the sprite to the real object.
		TurnReal();
		locked = LockAfterChange;

		hasColor = true;
		wolf.TransitionToColor(Color.white);
		return true;		
	}
	
	public bool CanTakeColor(bool printReason = false) {
		if (locked)
			return false;
		
		Color wolfColor = wolf.CurrentColor;
		
		//If the wolf already has a color, return.
		if(wolfColor != Color.white) {
			if (printReason)
				print ("Wolf already has color");
			return false;
		}
		
		if (!hasColor) {
			if (printReason)
				print ("Wolf cannot take color from object without color - " + name);
			//If the object is an empty drawing currently, do nothing.
			return false;
		}
		
		return true;
	}
	
	public bool TakeColor(){
		if (!CanTakeColor(true))
			return false;		
		
		wolf.TransitionToColor(RealColor);
		TurnDrawing();
		locked = LockAfterChange;		
		return true;		
	}
	
	public void Select(Color selectColor) {
		sr.color = selectColor;
	}
	
	public void Deselect() {
		sr.color = Color.white;
	}

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>(); 
		wolf = GameObject.FindWithTag("Wolf").GetComponent<Player>();
		
		if (StartAsDrawing)
			TurnDrawing();
		else
			TurnReal();
	}
	
	[Signal]
	void Delete() {
		GameObject.Destroy(gameObject);
	}
}
