using UnityEngine;
using System.Collections;

public class Cutscene2Dialogue : MonoBehaviour {
	GUIText textbox;
	string[] dialogue = new string[] {"I GUESS IT DOES WORK.",
	"THOSE RED ENEMIES HURT, \nEVEN THOUGH I CAN'T SEE THEM...",
	".....OH, THE PATH CLOSED BEHIND ME...",
	"I GUESS I'LL KEEP MOVING FORWARD.",
	"I FEEL A PULL FROM SOMEWHERE, MAYBE IT'S A MAGNET?",
	"[[Click on the yellow electromagnets to pull yourself toward them.]]"};
	int j = 0;
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
		//Vector3 screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		textbox = GameObject.Find ("dialogue").GetComponent<GUIText> () as GUIText;
		//print (textbox.pixelOffset);
		//textbox.pixelOffset = new Vector2(screenPos.x, screenPos.y + 30);
		//print (textbox.pixelOffset);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			if(j >= dialogue.Length) Application.LoadLevel(5); 
			if(!loading) StartCoroutine(nextDialogue());
		}
	}
}
