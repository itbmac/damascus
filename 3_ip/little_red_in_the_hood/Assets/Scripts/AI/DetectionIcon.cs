using UnityEngine;
using System.Collections;

public class DetectionIcon : MyMonoBehaviour {

	public Sprite Normal;
	public Sprite PlayerVisible;
	public Sprite PlayerDetected;
	Police police;
	Vector2 localPos;

	// Use this for initialization
	void Start () {
		police = GetComponentInParent<Police>();
		localPos = transform.position - transform.parent.position;
	}
	
	// Update is called once per frame
	void Update () {
		// override movement due to being a child object
		transform.rotation = Quaternion.identity;
		transform.position = (Vector2)transform.parent.position + localPos;
		
		if (police.CurrentState == Police.State.Normal)
			sprite = Normal;
		else if (police.CurrentState == Police.State.PlayerVisible)
			sprite = PlayerVisible;
		if (police.CurrentState == Police.State.PlayerDetected)
			sprite = PlayerDetected;		
	}
}
