using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

public class BoardManager : MonoBehaviour {
	public enum BoardSelector {NoLoad, FirstChildBoard, RandomBoard}
	
	public BoardSelector boardSelector = BoardSelector.NoLoad;
	public float Size = 2.95f;
	public Vector2 OffsetPixel = new Vector2(.45f, -1.2f);
	public GameObject HintObj;

	private int AdjustToInt(float f) {
		return Mathf.RoundToInt(.5f + (f / Size)) + 1;
	}	
	
	public GridCoord GetGridCoord(Vector2 pos) {
		pos -= OffsetPixel;
		
		if (pos.x < MacKenzieThreshold)
			pos.x += MacKenzieOffsetX;
		
		return new GridCoord(AdjustToInt(pos.x), AdjustToInt(pos.y));		
	}
	
	const float MacKenzieThreshold = -6.5f;
	const float MacKenzieSize = 2.7f;
	const float MacKenzieOffsetX = 1.75f;
	const float MacKenzieOffsetY = 0.15f;
	const float MacKenzieRandomness = 0.1f;
	
	public Vector2 GridCoord2Pos(GridCoord gc) {
		if (gc.x < 0)
			return new Vector2(
				MacKenzieSize * (gc.x - 1.5f) + OffsetPixel.x - MacKenzieOffsetX + UnityEngine.Random.Range(-1 * MacKenzieRandomness, MacKenzieRandomness),
				MacKenzieSize * (gc.y - 1.5f) + OffsetPixel.y - MacKenzieOffsetY + UnityEngine.Random.Range(-1 * MacKenzieRandomness, MacKenzieRandomness)
			);
		else
			return new Vector2(
				Size * (gc.x - 1.5f) + OffsetPixel.x,
				Size * (gc.y - 1.5f) + OffsetPixel.y
			);
	}
	
	public GridCoord SnapGridCoord(GridCoord gc) {
		gc.x = Mathf.Clamp(gc.x, -3, 3);
		if (gc.x < 0) {
			gc.y = Mathf.Clamp(gc.y, 0, 3);

			if ((gc.y == 3) && (gc.x == -3))
				gc.y = 2;
		}
		else
			gc.y = Mathf.Clamp(gc.y, 0, 3);
		return gc;
	}
	
	public List<GridCoord> GetAllPossibleBoardPositions() {
		var result = new List<GridCoord>();
		
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				result.Add(new GridCoord(i, j));
			}
		}
		
		return result;
	}
	
	public IEnumerable<GridCoord> GetAllPossibleSidePositions() {
		var result = new List<GridCoord>();
		
		for (int j = 0; j <= 2; j++)
			result.Add(new GridCoord(-3, j));
	
		for (int i = -2; i <= -1; i++) {
			for (int j = 0; j <= 3; j++) {
				result.Add(new GridCoord(i, j));
			}
		}
		
		return result;
	}
	
	public GameObject GetTileAtPosition(GridCoord pos, GameObject exceptObject) {
		GridCoord myGridCoord = SnapGridCoord(pos);
		// assumes only one tile at a position
		
		var tilesAtPosition = GameObject.FindGameObjectsWithTag("Tile").Where(
			tile => tile != exceptObject && GetGridCoord(tile.transform.position) == myGridCoord
		);
		
		return tilesAtPosition.FirstOrDefault();
	}
	
	public IEnumerable<GridCoord> FilterForOpenPositions(IEnumerable<GridCoord> positions, GameObject forObject) {
		// currently inefficient
		
		return positions.Where(x => IsPositionOpen(x.ToVector2(), forObject));
	}
	
	private Dictionary<GridCoord, GameObject> GetTileDictionary() {
		var result = new Dictionary<GridCoord, GameObject>();
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		
		// does not account for more than one piece at a GridCoord
		foreach (GameObject tile in tiles)
			result[tile.transform.position.ToGridCoord()] = tile;
			
		return result;		
	}
	
	private static Vector2? FindClosestPosition(Vector2 pos, IEnumerable<Vector2> positions) {
		Vector2? bestPosition = null;
		float bestDistance = Mathf.Infinity;
				
		foreach (var boardPosition in positions) {
			float distance = Vector2.Distance(boardPosition, pos);
			
			if (distance < bestDistance) {
				bestDistance = distance;
				bestPosition = boardPosition;
			}
		}
		
		return bestPosition	;
	}
	
	public Vector2? FindClosestOpenPositionOnBoard(Vector2 pos, GameObject forObject) {
		return FindClosestPosition(pos, FilterForOpenPositions(GetAllPossibleBoardPositions(), forObject).Select(x => x.ToVector2()));
	}
	
	public Vector2? FindClosestOpenPositionOnSide(Vector2 pos, GameObject forObject) {
		return FindClosestPosition(pos, FilterForOpenPositions(GetAllPossibleSidePositions(), forObject).Select(x => x.ToVector2()));
	}	
	
	public bool IsOutOfGame(Vector2 pos) {
		var gc = GetGridCoord(pos);
		var gcs = SnapGridCoord(gc);
		
		return gc != gcs;
	}
	
	public bool IsOnBoard(Vector2 pos) {
		var gc = GetGridCoord(pos);
		return 0 <= gc.x && gc.x < 4 && 0 <= gc.y && gc.y < 4;
	}
	
	public Vector2 SnapPos(Vector2 pos) {
		return GridCoord2Pos(GetGridCoord(pos));
	}
	
	public Vector2 SnapPosConstrained(Vector2 pos) {
		return GridCoord2Pos(SnapGridCoord(GetGridCoord(pos)));
	}
	
	GameObject[,] GetCurrentBoard() {
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		GameObject[,] board = new GameObject[4, 4];
		
		foreach (GameObject tile in tiles) {
			GridCoord gc = tile.transform.position.ToGridCoord();
			int x = gc.x;
			int y = gc.y;
			if (x < 0 || x > 3 || y < 0 || y > 3) {
				continue;
			}
			
			if (board[x,y] != null) {
//				Debug.LogWarning("Overlapping tiles: " + board[x,y].name + " and " + tile.name);
				continue;
			}
			board[x,y] = tile;
		}
		
		return board;
	}
	
	public bool IsPositionOpen(Vector3 pos, GameObject forObject) {
		GridCoord myGridCoord = SnapGridCoord(GetGridCoord(pos));
		
		return GameObject.FindGameObjectsWithTag("Tile").All (
			tile => tile == forObject || GetGridCoord(tile.transform.position) != myGridCoord
		);
	}
	
	public BoardState GetCurrentBoardState() {
		return BoardSolver.GetBoardState(GetCurrentBoard());
	}
	
	public bool IsCurrentBoardFilled() {
		GameObject[,] board = GetCurrentBoard();
		foreach (var t in board) {
			if (t == null)
				return false;
		}
	
		return true;
	}
	
	void GenerateBoard() {
		GameObject[,] board = BoardSolver.GenerateBoard(GetAllTiles());	
		Debug.Log ("Generated new board for designer, press G for more: \n" + BoardToString(board));
	}
	
	string BoardToString(GameObject[,] board) {
		// Note: does not print out as the physical board appears (specifically, prints out upside down)
		StringBuilder builder = new StringBuilder();
		builder.Append("[");
		for (int j = 0; j < board.GetLength(1); j++) {
			builder.Append("[");
			for (int i = 0; i < board.GetLength(0); i++) {
				var e = board[i, board.GetLength(1) - j - 1];
				builder.Append(e ? e.name : "_");
				if (i < board.GetLength(1) - 1)
					builder.Append(",\t");
			}
			builder.Append("]");
			if (j != board.GetLength(0) - 1) {
				builder.Append(",\n");
			}
		}
		builder.Append("]");
		return builder.ToString();
	}
	
	int CountSolutions() {
		return BoardSolver.CountSolutions(GetCurrentBoard(), GetSideTiles());
	}
	
	public bool PlaceValidTile() {
		var solvedBoard = GetSolvedBoard();
		var currentBoard = GetCurrentBoard();
		
		for (int i = 0; i < solvedBoard.GetLength(0); i++) {
			for (int j = 0; j < solvedBoard.GetLength(1); j++) {
				var currentTile = currentBoard[i,j];
				var solvedTile = solvedBoard[i,j];
				if (solvedTile == null)
					Debug.Log ("We gotta problem");
				if (solvedTile != currentTile) {
					if (currentTile != null) {
						currentTile.GetComponent<TileController>().MoveAndReset(solvedTile.transform.position);
					}
					
					solvedTile.GetComponent<TileController>().MoveAndReset((new GridCoord(i, j)).ToVector2());
					
					return true;
				}				
			}
		}	
		Debug.Log ("All good");
		
		return false;
	}
	
	public int ShakeInvalidTiles() {
		var invalidTiles = BoardSolver.GetInvalidTiles(GetCurrentBoard());
		
		foreach (var tile in invalidTiles) {
			tile.SendMessage("Shake");
		}
		
		return invalidTiles.Count();
	}
	
	public static BoardManager Instance;
	void Awake() {
		Instance = this;
	}
	
	List<int> randomBoards;
	int randomBoardIndex;
	
	private GameObject lastBoardContainer;
	private void LoadBoardFromObject(GameObject boardContainer) {
		lastBoardContainer = boardContainer;
		lastSolution = null;
		
		var newData = boardContainer.GetComponent<NewBoardData>();
		LoadFullBoard(newData.Board, newData.Side);
	}
	
	public void LoadLastBoard() {
		if (lastBoardContainer == null)
			Debug.LogError("No previous board to load");
		else
			LoadBoardFromObject(lastBoardContainer);
	}
	
	public void NewBoard() {
		if (boardSelector == BoardSelector.FirstChildBoard || boardSelector == BoardSelector.RandomBoard) {
			Transform premadeBoards = transform.Find("PremadeBoards");
			if (premadeBoards && premadeBoards.childCount > 0) {
				int boardIndex = 0;
				if (boardSelector == BoardSelector.RandomBoard) {
					if (randomBoards == null || randomBoardIndex >= randomBoards.Count) {
						randomBoards = Enumerable.Range(0, premadeBoards.childCount).AsRandom().ToList();
						randomBoardIndex = 0;
					}
					
					boardIndex = randomBoards[randomBoardIndex];
					randomBoardIndex += 1;
				}
				
				var boardContainer = premadeBoards.GetChild(boardIndex).gameObject;
				Debug.Log ("Loading new board... " + boardContainer.name + ", #" + boardIndex);
				LoadBoardFromObject(boardContainer);
			} else {
				Debug.LogWarning("Could not find board to load!");
			}
		}
		
		foreach (var tile in GameObject.FindGameObjectsWithTag("Tile")) {
			tile.GetComponent<TileController>().Reset();
		}

		HintObj.GetComponent<ClickForHint>().Reset();
	}

	void Start () {		
		GenerateBoard();
		
		NewBoard();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.G))
			GenerateBoard();
//		else if (Input.GetKeyDown(KeyCode.S))
//			NewBoard ();
		else if (Input.GetKeyDown(KeyCode.C)) {
			int numSolutions = BoardSolver.CountSolutions(GetCurrentBoard(), GetSideTiles());
			if (numSolutions == BoardSolver.MAX_SOLUTIONS_TO_FIND)
				Debug.Log(numSolutions + " or more possible solutions");
			else
				Debug.Log(numSolutions + " possible solutions");
		}
		else if (Input.GetKeyDown(KeyCode.S)) {
			var solved = BoardSolver.SolveBoard(GetCurrentBoard(), GetSideTiles());
			Debug.Log ("Solution to board as currently placed: \n" + BoardToString(solved));
		}
//		else if (Input.GetKeyDown(KeyCode.T))
//			ShakeInvalidTiles();
	}
	
	GameObject[] GetAllTiles() {
		return GameObject.FindGameObjectsWithTag("Tile");
	}
	
	IEnumerable<GameObject> GetSideTiles() {
		var sidePositions = GetAllPossibleSidePositions();
	
		return GetAllTiles().Where(x => sidePositions.Contains(x.transform.position.ToGridCoord()));		
	}
	
	
	void MoveAllTilesOffBoard() {
		foreach (var tile in GetAllTiles()) {
			tile.transform.position = new Vector2(-20, -20);
		}
	}
	
	GameObject[,] lastSolution;
	
	GameObject[,] GetSolvedBoard() {
		if (lastSolution != null)
			return lastSolution;
	
		var newData = lastBoardContainer.GetComponent<NewBoardData>();
		
		var sideTiles = JaggedArrayParser.Parse(newData.Side).SelectMany(x => x).Where(x => x != null);
		
		lastSolution = BoardSolver.SolveBoard(
			ParseBoard(newData.Board),
			sideTiles
		);
		
		return lastSolution;
	}
	
	GameObject[,] ParseBoard(string board) {
		var boardParsed = JaggedArrayParser.Parse(board);
		if (boardParsed.Length != 4)
			Debug.LogError("parse error, must have 4 rows per board");
		
		if (!boardParsed.All(x => x.Length == 4))
			Debug.LogError("parse error, each board row must have 4 entries");
		
		var boardFixed = new GameObject[4,4];
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				boardFixed[j,4 - i - 1] = boardParsed[i][j];
			}
		}
		
		return boardFixed;
	}
	
	void LoadFullBoard(string board, string side) {
		MoveAllTilesOffBoard();
		
		var boardParsed = JaggedArrayParser.Parse(board);
		if (boardParsed.Length != 4)
			Debug.LogError("parse error, must have 4 rows per board");
		
		if (!boardParsed.All(x => x.Length == 4))
			Debug.LogError("parse error, each board row must have 4 entries");
			
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				var go = boardParsed[i][j];
				if (go == null) continue;
				go.transform.position = (new GridCoord(j, 4 - i - 1)).ToVector2();
				go.SendMessage("Reset");
			}
		}
		
		var sideParsed = JaggedArrayParser.Parse(side);
		if (sideParsed.Length != 4)
			Debug.LogError("parse error, must have 4 rows on the side");
		
		for (int i = 0; i <= 3; i++) {
//			if (false) { //(i == 3) {
//				for (int j = -2; j <= -1; j++) {
//					var go = sideParsed[i][j + 2];
//					if (go == null) continue;
//					go.transform.position = (new GridCoord(i, j)).ToVector2();
//				}
//			} else {
				for (int j = -3; j <= -1; j++) {
					var go = sideParsed[i][j + 3];
					if (go == null) continue;
					go.transform.position = (new GridCoord(j, 4 - i - 1)).ToVector2();
				}
//			}
		}
	}
}
