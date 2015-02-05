using UnityEngine;
using System.Collections;

public class Cutscene1Dialogue : MonoBehaviour {
	GUIText textbox;
	string[] dialogue = new string[] {".........",
		"......WHERE AM I?",
		"CAN'T SEE...OR SENSE ANYTHING...",
		"WHAT DO I DO NOW?",
		"......................",
		"I CAN FEEL MY ECHOLOCATOR.",
		"I WONDER IF IT WILL WORK?",
		"[[WASD to move.]]",
		"[[press space to try out the echolocator.]]"};
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
		Vector3 screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		textbox = gameObject.GetComponent<GUIText>();
		print (textbox.pixelOffset);
		textbox.pixelOffset = new Vector2(screenPos.x, screenPos.y + 30);
		print (textbox.pixelOffset);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			if(j >= dialogue.Length) Application.LoadLevel(3); 
			if(!loading) StartCoroutine(nextDialogue());
		}
	}
}
