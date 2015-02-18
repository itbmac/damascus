using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum BoardState {Valid, Incomplete, InvalidPair, InvalidCounts}

public static class BoardSolver {
	public const int MAX_SOLUTIONS_TO_FIND = 1000;
	
	public static GameObject[,] SolveBoard(GameObject[,] board, IEnumerable<GameObject> tiles) {
		GameObject[,] solved = (GameObject[,])board.Clone();
		if (!SolveBoard(solved, 0, 0, tiles))
			Debug.LogWarning("Unable to solve board!");
		
		return solved;
	}
	
	public static GameObject[,] GenerateBoard(IEnumerable<GameObject> tiles) {
		GameObject[,] board = new GameObject[4,4];
		if (!SolveBoard(board, 0, 0, tiles.AsRandom()))
			Debug.LogError("Unable to generate board!");
			
		return board;
	}
	
	private static bool SolveBoard(GameObject[,] board, int i, int j, IEnumerable<GameObject> sideTiles) {	
		if (j >= board.GetLength(1)) {
			i += 1;
			j = 0;
		}
		if (i >= board.GetLength(0)) {
			return true;
		}
		
		if (board[i,j] != null)	
			return SolveBoard(board, i, j + 1, sideTiles);
		
		GameObject original = board[i,j];
		foreach (GameObject t in sideTiles) {
			var newSideTiles = sideTiles.Except(new [] {t});
			
			board[i,j] = t;
			if (GetProvisionalBoardState(board) != BoardState.Valid)
				continue;
				
			if (SolveBoard(board, i, j + 1, newSideTiles)) {
				return true;
			}
		}
		board[i,j] = original;
		
		return false;
	}

	public static int CountSolutions(GameObject[,] board, IEnumerable<GameObject> sideTiles) {
		return CountSolutions(board, 0, 0, MAX_SOLUTIONS_TO_FIND, sideTiles);
	}

	private static int CountSolutions(GameObject[,] board, int i, int j, int maxSolutions, IEnumerable<GameObject> sideTiles) {	
		// Counts the number of possible solutions formed by filling in the remaining
		// open slots on the board. 
		
		if (j >= board.GetLength(1)) {
			i += 1;
			j = 0;
		}
		if (i >= board.GetLength(0)) {
			return 1;
		}
		
		if (board[i,j] != null)	
			return CountSolutions(board, i, j + 1, maxSolutions, sideTiles);
		
		
		var d = new Dictionary<GameObject, bool>();
		foreach (var t in board) {
			if (t != null)
				d[t] = true;
		}
		
		GameObject original = board[i,j];
		
		int totalSolutions = 0;
		foreach (GameObject t in sideTiles) {
			var newSideTiles = sideTiles.Except(new [] {t});
		
			board[i,j] = t;
			if (GetProvisionalBoardState(board) == BoardState.Valid) {
				totalSolutions += CountSolutions(board, i, j + 1, maxSolutions - totalSolutions, newSideTiles);
			} 
			
			// for performance reasons, if there's too many solutions break out of everything
			if (totalSolutions >= maxSolutions)
				break;
		}
		
		board[i,j] = original;
		return totalSolutions;
	}
	
	public static BoardState GetBoardState(GameObject[,] board) {
		foreach (var tile in board) {
			if (tile == null)
				return BoardState.Incomplete;
		}
		
		return GetProvisionalBoardState(board);
	}
	
	private static int Classify(GameObject g) {
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
	
	public static BoardState GetProvisionalBoardState(GameObject[,] board) {
		IDictionary<GameObject, GridCoord> tileCoords = new Dictionary<GameObject, GridCoord>();
		
		for (int i = 0; i < board.GetLength(0); i++) {
			for (int j = 0; j < board.GetLength(1); j++) {
				var e = board[i,j];
				if (e != null) {
					tileCoords[e] = new GridCoord(i, j);
				}
			}
		}
		
		InvalidPair[] invalidPairs = MonoBehaviour.FindObjectsOfType<InvalidPair>();
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
					// Debug.Log ("Invalid pair: " + someTile.name + " and " + blockedTile.name);
					return BoardState.InvalidPair;
				}
			}
		}
		
		for (int x = 0; x < 4; x++) {
			int[] test = new int[4];
			int[] test2 = new int[4];
			
			for (int y = 0; y < 4; y++) {				
				if (board[x,y] != null)
					test[Classify(board[x,y])] += 1;
				if (board[y,x] != null)
					test2[Classify(board[y,x])] += 1;
			}
			
			if (!test.All(k => k <= 1)) {
				//				Debug.Log ("Fail col " + x);
				//				Debug.Log (string.Join(",", test.Select(k => k.ToString()).ToArray()));
				return BoardState.InvalidCounts;
			}
			
			if (!test2.All(k => k <= 1)) {
				//				Debug.Log ("Fail row " + x);
				//				Debug.Log (string.Join(",", test2.Select(k => k.ToString()).ToArray()));
				return BoardState.InvalidCounts;
			}
		}
		
		return BoardState.Valid;
	}
	
	public static IEnumerable<GameObject> GetInvalidTiles(GameObject[,] board) {
		var result = new HashSet<GameObject>();
		
		IDictionary<GameObject, GridCoord> tileCoords = new Dictionary<GameObject, GridCoord>();
		
		for (int i = 0; i < board.GetLength(0); i++) {
			for (int j = 0; j < board.GetLength(1); j++) {
				var e = board[i,j];
				if (e == null) {
					
				} else {
					tileCoords[e] = new GridCoord(i, j);
				}
			}
		}
		
		InvalidPair[] invalidPairs = MonoBehaviour.FindObjectsOfType<InvalidPair>();
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
					result.Add(someTile);
					result.Add(blockedTile);
				}
			}
		}
		
		for (int x = 0; x < 4; x++) {
			List<GameObject>[] testList = new [] {new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>()};
			List<GameObject>[] testList2 = new [] {new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>()};
			
			for (int y = 0; y < 4; y++) {
				var e1 = board[x,y];
				var e2 = board[y,x];
				
				if (e1 != null)
					testList[Classify(e1)].Add(e1);
				
				if (e2 != null)
					testList2[Classify(e2)].Add(e2);
			}
			
			foreach (var t in testList) {
				if (t.Count > 1)
					result.UnionWith(t);
			}
			
			foreach (var t in testList2) {
				if (t.Count > 1)
					result.UnionWith(t);
			}
		}
		
		return result;
	}
}
