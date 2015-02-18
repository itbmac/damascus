using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using InvalidOperationException = System.InvalidOperationException;

public static class Extensions {

	public static IEnumerable<T> AsRandom<T>(this IEnumerable<T> sequence)
	{
		T[] retArray = sequence.ToArray();
		
		for (int i = 0; i < retArray.Length - 1; i += 1)
		{
			int swapIndex = Random.Range(i, retArray.Length);
			
			if (swapIndex != i) {
				// don't waste time swapping an object to its current position
				
				T temp = retArray[i];
				retArray[i] = retArray[swapIndex];
				retArray[swapIndex] = temp;
			}
		}
		
		return retArray;
	}
	
	public static GridCoord ToGridCoord(this Vector2 pos) {
		return BoardManager.Instance.GetGridCoord(pos);
	}
	
	public static GridCoord ToGridCoord(this Vector3 pos) {
		return BoardManager.Instance.GetGridCoord(pos);
	}
	
	public static Vector2 ToVector2(this GridCoord gridCoord) {
		return BoardManager.Instance.GridCoord2Pos(gridCoord);
	}
	
	public static T[,] To2D<T>(this T[][] source)
	{
		try
		{
			int FirstDim = source.Length;
			int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular
			
			var result = new T[FirstDim, SecondDim];
			for (int i = 0; i < FirstDim; ++i)
				for (int j = 0; j < SecondDim; ++j)
					result[i, j] = source[i][j];
			
			return result;
		}
		catch (InvalidOperationException)
		{
			throw new InvalidOperationException("The given jagged array is not rectangular.");
		} 
	}
}
