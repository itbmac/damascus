using System;
using UnityEngine;

[RequireComponent(typeof(ColorToggle))]
public abstract class Colorable : MonoBehaviour
{
	public abstract void OnTurnReal();	
	public abstract void OnTurnDrawing();
}


