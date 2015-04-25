using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class TimeElapsed : MonoBehaviour {

	public GameObject TargetSystemTracker;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		int numRemainingDrops = TargetSystemTracker.GetComponent<TargetSystem>().numTargets - TargetSystemTracker.GetComponent<TargetSystem>().currentTarget;
		if (numRemainingDrops != 1)
			GetComponent<Text>().text = "";// NumberToWords(numRemainingDrops) + "  Drops  Left"; //NumberToWords(Mathf.RoundToInt(Time.time));
		else
			GetComponent<Text>().text = ""; // NumberToWords(numRemainingDrops) + "  Drop  Left";
	}

	string NumberToWords(int number)
	{
		if (number == 0)
			return "Zero";
		
		if (number < 0)
			return "Minus " + NumberToWords(Mathf.Abs(number));
		
		string words = "";
		
		if ((number / 1000000) > 0)
		{
			words += NumberToWords(number / 1000000) + " Million ";
			number %= 1000000;
		}
		
		if ((number / 1000) > 0)
		{
			words += NumberToWords(number / 1000) + " Thousand ";
			number %= 1000;
		}
		
		if ((number / 100) > 0)
		{
			words += NumberToWords(number / 100) + " Hundred ";
			number %= 100;
		}
		
		if (number > 0)
		{
			if (words != "")
				words += "and ";
			
			var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
			var tensMap = new[] { "zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
			
			if (number < 20)
				words += unitsMap[number];
			else
			{
				words += tensMap[number / 10];
				if ((number % 10) > 0)
					words += "-" + unitsMap[number % 10];
			}
		}
		
		return words;
	}
}
