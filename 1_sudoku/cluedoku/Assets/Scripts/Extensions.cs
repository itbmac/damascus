using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public static class Extensions {

	static Random random = new Random();

	public static IEnumerable<T> AsRandom<T>(this IEnumerable<T> sequence)
	{
		T[] retArray = sequence.ToArray();
		
		for (int i = 0; i < retArray.Length - 1; i += 1)
		{
			int swapIndex = random.Next(i, retArray.Length);
			if (swapIndex != i) {
				T temp = retArray[i];
				retArray[i] = retArray[swapIndex];
				retArray[swapIndex] = temp;
			}
		}
		
		return retArray;
	}
}
