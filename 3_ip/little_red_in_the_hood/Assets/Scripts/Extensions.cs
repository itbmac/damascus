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
	
	public static Vector2 Rotated(this Vector2 v, float radians) {
		var angle = v.Angle() + radians;
		return v.magnitude * UnitVectorForAngle(angle);
	}
	
	public static Vector2 UnitVectorForAngle(float radians) {
		return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
	}
	
	public static IEnumerable<T> Prepend<T>(this IEnumerable<T> seq, T val) {
		yield return val;
		foreach (T t in seq) {
			yield return t;
		}
	}
	
	public static Vector2 RayLineIntersection(Vector2 r_p, Vector2 r_d, Vector2 s_p, Vector2 s_d) {		
		float T2 = (r_d.x*(s_p.y-r_p.y) + r_d.y*(r_p.x-s_p.x))/(s_d.x*r_d.y - s_d.y*r_d.x);
		float T1 = (s_p.x+s_d.x*T2-r_p.x)/r_d.x;
		if (T1 < 0)
			Debug.LogError("Ray doesn't intersect line");
		
		return r_p + r_d * T1;		
	}
	
	public static float Distance(this Transform t, Transform other) {
		return Vector2.Distance(t.position, other.position);
	}
}
