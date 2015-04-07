using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistractorCount : MonoBehaviour {

	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = Player.Instance.NumPaint + " Spraypaint\n"
			+ Player.Instance.NumGlowsticks + " Glowsticks\n";
	}
}
