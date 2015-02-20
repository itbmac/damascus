using UnityEngine;
using System.Collections;

public class OverlayVerifyTilePlacement : MonoBehaviour {

	public GameObject tile;
	public Vector2 desiredTilePlacement;
	public Vector2 curTilePlacement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			GridCoord curTileInfo = BoardManager.Instance.GetGridCoord(new Vector2(tile.transform.position.x, tile.transform.position.y)); 
			curTilePlacement = new Vector2(curTileInfo.x, curTileInfo.y); 
			if ((curTilePlacement.x == desiredTilePlacement.x) && (curTilePlacement.y == desiredTilePlacement.y)) {
				gameObject.SetActive(false);
			}
		}
	}
}
