using UnityEngine;
using System.Collections;

public static class Extensions {

	public static Vector3 ToVector(this Color color) {
		return new Vector3(color.r, color.g, color.b);
	}
}
