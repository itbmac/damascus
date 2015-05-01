using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {

	public static TutorialText Instance {
		get; private set;
	}
	
	Text myText;
	
	void Awake() {
		Instance = this;	
	}

	// Use this for initialization
	void Start () {
		myText = GetComponent<Text>();
		myText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void TriggerText(string text) {
		myText.text = text ?? "";
	}
}
