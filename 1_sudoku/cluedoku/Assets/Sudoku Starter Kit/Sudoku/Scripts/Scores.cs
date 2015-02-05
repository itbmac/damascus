using UnityEngine;
using System.Collections;

public class Scores : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
			string es = PlayerPrefs.GetString("easyscore","99:59:59");					// get easy, medium and hard high scores
			string ms = PlayerPrefs.GetString("mediumscore","99:59:59");
			string hs = PlayerPrefs.GetString("hardscore","99:59:59");
			
			GameObject.Find("EasyScore").guiText.text = es;									// display scores
			GameObject.Find("MediumScore").guiText.text = ms;
			GameObject.Find("HardScore").guiText.text = hs;
		}
}

