using UnityEngine;
using System.Collections;

public class MoveTilePositionBeforeGame : MonoBehaviour {

	public GameObject verifyOkayToMoveOffOfObj;
	public Vector2 desiredTilePlacement;
	public Vector2 desiredTilePlacementLoc;
	public bool hasMoved = false;

	// Use this for initialization
	void Start () {
		desiredTilePlacementLoc = BoardManager.Instance.GridCoord2Pos(new GridCoord((int)desiredTilePlacement.x, (int)desiredTilePlacement.y));
	}
	
	// Update is called once per frame
	void Update () {
		if (verifyOkayToMoveOffOfObj && verifyOkayToMoveOffOfObj.activeSelf) {
			gameObject.GetComponent<TileController>().Move(desiredTilePlacementLoc);
			hasMoved = true;
			this.enabled = false;
		}
	}
}
