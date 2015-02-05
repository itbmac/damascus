using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;

public class UI : MonoBehaviour 
{
	public BatteryBar batteryBar;
	public static UI reference;
	float timeSinceRecharge = 0.0f;
	public float timeToRecharge = 1.5f;
	
	// Use this for initialization
	void Start () {
		Texture2D texture = (Texture2D)Resources.Load("battery");
		batteryBar = new BatteryBar(texture, 1);
		reference = this;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeSinceRecharge += Time.deltaTime;
		
		if (timeSinceRecharge >= timeToRecharge)
		{
			timeSinceRecharge = 0.0f;
			rechargeBattery();
		}
	}
	
	public void addBattery ()
	{
		batteryBar.maxBatteries += 1;
	}
	
	void rechargeBattery ()
	{
		StackTrace stackTrace = new StackTrace();
		UnityEngine.Debug.Log(stackTrace.GetFrame(1).GetMethod().Name);
		if (batteryBar.batteries < batteryBar.maxBatteries)
		{
			batteryBar.batteries += 1;
		}
	}
	
	public void removeBattery ()
	{
		batteryBar.batteries -= 1;
	}
	
	void OnGUI ()
	{ 
		batteryBar.display();
	}
	
	public class BatteryBar
	{
		public int batteries;
		public Texture2D batteryTexture;
		public int offset;
		public int pad = 20;
		public int maxBatteries;
		
		public BatteryBar (Texture2D t, int b)
		{
			batteries = b;
			batteryTexture = t;
			offset = batteryTexture.width + 10;
			maxBatteries = 3;
		}
		
		public void display()
		{
			for (int i = batteries; i > 0; i--)
			{
				float x1 = Camera.main.pixelWidth - offset*i - pad;
				float y1 = pad;
				
				Rect pos = new Rect(x1, y1, 19, 7);
				GUI.DrawTexture(pos, batteryTexture);
			}
		}
	}
}
