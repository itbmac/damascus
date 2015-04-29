using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RegionText : MyMonoBehaviour 
{
	Text text;
	public enum States {crossedRegion, fading, faded};
	public States state;
	public float fadePerFrame;
	public float timeToFade;
	Player player;
	
	IEnumerator DisplayRegionCoroutine()
	{
		string lastRegion = "";
		string currentRegion = player.FindCurrentRegion();
		
		while (true)
		{
			currentRegion = player.FindCurrentRegion();
			
			if (state ==  States.fading)
			{	
				Color oldColor = text.color;
				Color newColor = oldColor;
				newColor.a -= fadePerFrame;
				text.color = newColor;
				
				if (newColor.a <= 0.0f)
				{
					state = States.faded;
				}
			}
			else if (state == States.faded)
			{
				if (currentRegion != lastRegion && currentRegion.Length >= 1)
				{
					lastRegion = currentRegion;
					text.text = currentRegion;
					
					Color oldColor = text.color;
					Color newColor = oldColor;
					newColor.a = 1.0f;
					text.color = newColor;
					
					yield return new WaitForSeconds(timeToFade);
					state = States.fading;
				}
			}
			
			yield return null;
		}
	}
	
	// Use this for initialization
	void Start () {
		state = States.faded;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		text = GetComponent<Text>();
		StartCoroutine(DisplayRegionCoroutine());
	}
	
	// Update is called once per frame
	void Update () {

	}
}
