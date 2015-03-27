using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Diety : MonoBehaviour {

	ColorToggle[] childs;
	
	bool nextLevel;
	SpriteRenderer fade;

	// Use this for initialization
	void Start () {
		childs = GetComponentsInChildren<ColorToggle>();
		fade = GameObject.Find ("FadeToWhite").GetComponent<SpriteRenderer>() as SpriteRenderer;
	}

	IEnumerator FadeWhite(){
		float i = 0;
		float alpha = 0;
		Color temp;
		while(i < 3){
			alpha += Time.deltaTime/3;
			temp = new Color(1,1,1,alpha);
			fade.color = temp;
			yield return null;
		}
	}

	// Update is called once per frame
	void Update () {
	
		if (!nextLevel && childs.All(x => x.hasColor)){
			Application.LoadLevel(Application.loadedLevel + 1);
			nextLevel = true;

			//Start whiteout.
			//StartCoroutine("FadeWhite");
			//Application.LoadLevel(Application.loadedLevel + 1);
		}

		/*
		if (Input.GetKeyDown("f")) {
			StartCoroutine("FadeWhite");
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		*/
	}
}
