using UnityEngine;
using System.Collections;
using System.Linq;

public class BoardManager : MonoBehaviour {

	public AudioClip noiseWin;
	const float SIZE = 2.5f;

	private int AdjustToInt(float f) {
		return Mathf.RoundToInt(.5f + f / SIZE) + 1;
	}
	
	private int Classify(GameObject g) {
		string n = g.transform.parent.name;
		if (n == "Suspects")
			return 0;
		else if (n == "Locations")
			return 1;
		else if (n == "Victims")
			return 2;
		else if (n == "Weapons")
			return 3;
		
		Debug.LogError("unknown " + n);
		return 4;
	}

	bool CheckBoard() {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		GameObject[,] board = new GameObject[4, 4];
		
		foreach (GameObject tile in tiles) {
			Vector3 pos = tile.transform.position;
			int x = AdjustToInt(pos.x);
			int y = AdjustToInt(pos.y);
			if (x < 0 || x > 3 || y < 0 || y > 3) {
				continue;
			}
			
			if (board[x,y] != null) {
				Debug.Log ("duplicate tile");
				return false;
			}
			board[x,y] = tile;
		}
		
		InvalidPair[] invalidPairs = gameObject.GetComponentsInChildren<InvalidPair>();
		foreach (InvalidPair invalidPair in invalidPairs) {
			GameObject someTile = invalidPair.SomeTile;
			Vector3 pos = someTile.transform.position;
			int someTilex = AdjustToInt(pos.x);
			int someTiley = AdjustToInt(pos.y);
			if (someTilex < 0 || someTilex > 3 || someTiley < 0 || someTiley > 3) {
				continue;
			}
			
			foreach (GameObject blockedTile in invalidPair.BlockedTiles) {
				Vector3 blockedTilePos = blockedTile.transform.position;
				int blockedTilex = AdjustToInt(blockedTilePos.x);
				int blockedTiley = AdjustToInt(blockedTilePos.y);
				if (blockedTilex < 0 || blockedTilex > 3 || blockedTiley < 0 || blockedTiley > 3) {
					continue;
				}
				
				if (someTilex == blockedTilex || someTiley == blockedTiley) {
					Debug.Log ("Illegal pair found: " + someTile.name + " " + blockedTile.name);
					return false;
				}
			}
		}
		
//		Transform invalidPairsContainer = transform.Find("InvalidPairs");
//		foreach (Transform invalidPairTransform in invalidPairsContainer) {
//			GameObject invalidPair = 
//		}
		
//		foreach (GameObject tile in tiles) {
//			foreach (Transform invalidPairTransform in GetChild)
//		
//			Vector3 pos = tile.transform.position;
//			int x = AdjustToInt(pos.x);
//			int y = AdjustToInt(pos.y);
//			if (x < 0 || x > 3 || y < 0 || y > 3) {
//				continue;
//			}
//			
//			for (int y2 = 0; y2 < 3; y2++) {
//				if (y2 == y)
//					continue;
//				
//			}
//		}
		
		for (int x = 0; x < 4; x++) {
			int[] test = new int[4];
			int[] test2 = new int[4];
			
			for (int y = 0; y < 4; y++) {
				if (board[x,y] == null || board[y,x] == null)
					return false;
					
				test[Classify(board[x,y])] += 1;
				test2[Classify(board[y,x])] += 1;
			}
			
			if (test[0] != 1 || test[1] != 1 || test[2] != 1 || test[3] != 1) {
				Debug.Log ("Fail col " + x);
				Debug.Log (string.Join(",", test.Select(k => k.ToString()).ToArray()));
				return false;
			}
			
			if (test2[0] != 1 || test2[1] != 1 || test2[2] != 1 || test2[3] != 1) {
				Debug.Log ("Fail row " + x);
				Debug.Log (string.Join(",", test2.Select(k => k.ToString()).ToArray()));
				return false;
			}
		}

		if (!audio.isPlaying)
			audio.PlayOneShot(noiseWin);
		return true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			Debug.Log (CheckBoard());
		}
	}
	
	public void Submit() {
		if (CheckBoard()) {
			Debug.Log ("Valid board.");
		} else {
			Debug.Log ("Submitted. Invalid board.");
		}
	}
}
