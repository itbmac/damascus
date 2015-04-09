using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Extensions {

	public static Vector2 Abs(this Vector2 vector) {
		for (int i = 0; i < 2; ++i) vector[i] = Mathf.Abs(vector[i]);
		return vector;
	}   
	
	public static Vector2 DividedBy(this Vector2 vector, Vector2 divisor) {
		for (int i = 0; i < 2; ++i) vector[i] /= divisor[i];
		return vector;
	}
	
	public static Vector2 Max(this Rect rect) {
		return new Vector2(rect.xMax, rect.yMax);
	}
	
	public static Vector2 IntersectionWithRayFromCenter(Vector2 boxMin, Vector2 boxMax, Vector2 pointOnRay) {	
		Vector2 boxCenter = (boxMin + boxMax)/2;
	
		Vector2 pointOnRayLocal = pointOnRay - boxCenter;
		Vector2 edgeToRayRatios = (boxMax - boxCenter).DividedBy(pointOnRayLocal.Abs());
		return (edgeToRayRatios.x < edgeToRayRatios.y) ?
			new Vector2(pointOnRayLocal.x > 0 ? boxMax.x : boxMin.x, 
			            pointOnRayLocal.y * edgeToRayRatios.x + boxCenter.y) :
				new Vector2(pointOnRayLocal.x * edgeToRayRatios.y + boxCenter.x, 
				            pointOnRayLocal.y > 0 ? boxMax.y : boxMin.y);
	}
	
	public static float Angle(this Vector2 v) {
		return Mathf.Atan2(v.y, v.x);	
	}
	
//	public static float Angle(this Vector3 v) {
//		return Mathf.Atan2 (v.y, v.x);	
//	}
	
	public static Vector2 Rotated(this Vector2 v, float addAngle) {
		var angle = v.Angle() + addAngle;
		return v.magnitude * UnitVectorForAngle(angle);
	}
	
	public static Vector2 UnitVectorForAngle(float angle) {
		return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
	}
	
	public static IEnumerable<T> Prepend<T>(this IEnumerable<T> seq, T val) {
		yield return val;
		foreach (T t in seq) {
			yield return t;
		}
	}
}
