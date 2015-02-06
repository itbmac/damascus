using UnityEngine;
using System.Collections;

public class InvalidPair : MonoBehaviour {

	public GameObject SomeTile;
	public GameObject[] BlockedTiles;
	
	void Start() {
		if (SomeTile == null)
			Debug.LogError("No SomeTile");
		if (BlockedTiles.Length == 0)
			Debug.LogError("No Blocked Tiles");
	}
}
