using UnityEngine;
using System.Collections;

public class SightActivate : MonoBehaviour {

	GUIText textbox;
	string[] dialogue = new string[] {"THIS AUGMENT...",
		"I THINK... I CAN USE THIS TO SEE!",
		"FINALLY......",
		"MAYBE I'LL FIND MY PURPOSE HERE."};
	int j = 1;
	bool loading = false;
	
	IEnumerator nextDialogue(){
		loading = true;
		Color col = textbox.color;
		
		//fade out old text
		for(float i = 1f; i > 0f; i -=.03f){
			col.a = i;
			textbox.color = col;
			yield return null;
		}
		
		textbox.text = dialogue[j];
		j++;
		
		//fade in new text
		for(float i = 0f; i <= 1f; i +=.03f){
			col.a = i;
			textbox.color = col;
			yield return null;
		}
		loading = false;
	}
	
	// Use this for initialization
	void Start () {
		Vector3 screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		textbox = GameObject.Find("dialogue").GetComponent<GUIText>() as GUIText;
		//textbox.pixelOffset = new Vector2(screenPos.x, screenPos.y + 30);
		textbox.pixelOffset = new Vector2(0, -20);
		textbox.text = dialogue[0];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			if(j >= dialogue.Length) Application.LoadLevel(2); //Maybe need a cutscene in between here 
			if(!loading) StartCoroutine(nextDialogue());
		}
	}
}
