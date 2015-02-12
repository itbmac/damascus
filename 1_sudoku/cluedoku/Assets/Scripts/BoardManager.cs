using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using MiniJSON;
using System.Linq;

public struct GridCoord {
	public int x;
	public int y;

	public GridCoord(int x, int y) {
		if (x < 0 || x > 3 || y < 0 || y > 3)
			Debug.LogError("Invalid GridCoord " + x + " " + y);
		
		this.x = x;
		this.y = y;
	}
}

public class BoardManager : MonoBehaviour {

	public AudioClip noiseWin;
	public AudioClip noiseLose;
	public GameObject failedArrestWarrent;
	public bool LoadBoardOnStart = true;
	const float SIZE = 2.95f;//.5f;

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
	
	GameObject[,] GetCurrentBoard() {
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
				Debug.LogWarning("duplicate tile");
				continue;
			}
			board[x,y] = tile;
		}
		
		return board;
	}
	
	bool CheckCurrentBoard() {
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
	
		return CheckBoard(board);
	}
	
	bool IsCurrentBoardFilled() {
		GameObject[,] board = GetCurrentBoard();
		foreach (var t in board) {
			if (t == null)
				return false;
		}
	
		return true;
	}
	
	void GenerateBoard() {
		GameObject[,] board = new GameObject[4,4];
		GenerateBoard(board, 0, 0);
	}
	
	void PrintBoard(GameObject[,] board) {
		StringBuilder builder = new StringBuilder();
		builder.Append("[");
		for (int i = 0; i < board.GetLength(0); i++) {
			builder.Append("[");
			for (int j = 0; j < board.GetLength(1); j++) {
				var e = board[i,j];
				builder.Append(e.name);
				if (j < board.GetLength(1) - 1)
					builder.Append(",\t");
			}
			builder.Append("]");
			if (i != board.GetLength(0) - 1) {
				builder.Append(",\n");
			}
		}
		builder.Append("]");
		Debug.Log(builder.ToString());
	}
	
	bool GenerateBoard(GameObject[,] board, int i, int j) {	
		if (j >= board.GetLength(1)) {
			i += 1;
			j = 0;
		}
		if (i >= board.GetLength(0)) {
			PrintBoard(board);
			return true;
		}

		var d = new Dictionary<GameObject, bool>();
		foreach (var t in board) {
			if (t != null)
				d[t] = true;
		}

		var tilesRemaining = GameObject.FindGameObjectsWithTag("Tile").Where(x => !d.ContainsKey(x)).AsRandom();
		foreach (GameObject t in tilesRemaining) {
			board[i,j] = t;
			if (CheckBoard(board, true)) {
				if (GenerateBoard(board, i, j + 1))
					return true;
			} 
		}
		
		board[i,j] = null;
		return false;
	}

	bool CheckBoard(GameObject[,] board, bool provisional = false) {
		IDictionary<GameObject, GridCoord> tileCoords = new Dictionary<GameObject, GridCoord>();
		
		for (int i = 0; i < board.GetLength(0); i++) {
			for (int j = 0; j < board.GetLength(1); j++) {
				var e = board[i,j];
				if (e != null)
					tileCoords[e] = new GridCoord(i, j);
			}
		}
	
		// TODO: build tileCoords 
		InvalidPair[] invalidPairs = gameObject.GetComponentsInChildren<InvalidPair>();
		foreach (InvalidPair invalidPair in invalidPairs) {
			GameObject someTile = invalidPair.SomeTile;
			if (!tileCoords.ContainsKey(someTile))
				continue;
			
			GridCoord pos = tileCoords[someTile];
			foreach (GameObject blockedTile in invalidPair.BlockedTiles) {
				if (!tileCoords.ContainsKey(blockedTile))
					continue;
			
				GridCoord blockedTilePos = tileCoords[blockedTile];
				
				if (pos.x == blockedTilePos.x || pos.y == blockedTilePos.y) {
//					Debug.Log ("Illegal pair found: " + someTile.name + " " + blockedTile.name);
					return false;
				}
			}
		}
		
		for (int x = 0; x < 4; x++) {
			int[] test = new int[4];
			int[] test2 = new int[4];
			
			for (int y = 0; y < 4; y++) {
				if (!provisional && (board[x,y] == null || board[y,x] == null)) {
					return false;
				}
				
				if (board[x,y] != null)
					test[Classify(board[x,y])] += 1;
				if (board[y,x] != null)
					test2[Classify(board[y,x])] += 1;
			}
			
			if (!test.All(k => k == 1 || (provisional && k <= 1))) {
//				Debug.Log ("Fail col " + x);
//				Debug.Log (string.Join(",", test.Select(k => k.ToString()).ToArray()));
				return false;
			}
			
			if (!test2.All(k => k == 1 || (provisional && k <= 1))) {
//				Debug.Log ("Fail row " + x);
//				Debug.Log (string.Join(",", test2.Select(k => k.ToString()).ToArray()));
				return false;
			}
		}

		return true;
	}

	// Use this for initialization
	void Start () {
		Debug.Log (DumpBoard());
		
		Transform premadeBoards = transform.Find("PremadeBoards");
		if (LoadBoardOnStart && premadeBoards && premadeBoards.childCount > 0) {
			LoadBoard(premadeBoards.GetChild(0).GetComponent<BoardData>().Data);
		}
		
		GenerateBoard();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.G))
			GenerateBoard();
			
		Debug.Log (IsCurrentBoardFilled());
	}
	
	public void Submit() {
		if (CheckCurrentBoard()) {
			Debug.Log ("Valid board.");
			if (!audio.isPlaying)
				audio.PlayOneShot(noiseWin);
				
		} else {
			Debug.Log ("Submitted. Invalid board.");
			failedArrestWarrent.renderer.enabled = true;
			audio.PlayOneShot(noiseLose);
		}
	}
	
	void LoadBoard(string board) {
		var tiles = Json.Deserialize(board) as Dictionary<string, object>;
		
		foreach (var pair in tiles) {
			string name = pair.Key;
			var pos = pair.Value as Dictionary<string, object>;
			var x = (double)pos["x"];
			var y = (double)pos["y"];
			
			var go = GameObject.Find(name);
			go.transform.position = new Vector2((float)x, (float)y);
		}
	}
	
	string DumpBoard() {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		var tilesDict = new Dictionary<string, object>();
		
		foreach (GameObject tile in tiles) {
			tilesDict[tile.name] = new Dictionary<string, double> {
				{"x", tile.transform.position.x},
				{"y", tile.transform.position.y},
			};
		}
		
		return Json.Serialize(tilesDict);
	}
}
