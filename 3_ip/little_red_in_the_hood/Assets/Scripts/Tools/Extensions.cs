using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct Vector2d {
	public double x, y;
	
	public Vector2d(double x, double y) {
		this.x = x;
		this.y = y;
	}
	
	public Vector2d Abs() {
		Vector2d vector = this;
		vector.x = Math.Abs (vector.x);
		vector.y = Math.Abs (vector.y);
		return vector;
	}
	
	public Vector2d DividedBy(Vector2d divisor) {
		Vector2d vector = this;
		vector.x /= divisor.x;
		vector.y /= divisor.y;
		return vector;
	}
	
	public static implicit operator Vector2d(Vector2 v) {
		return new Vector2d(v.x, v.y);
	}
	
	public static explicit operator Vector2d(Vector3 v) {
		return new Vector2d(v.x, v.y);
	}
	
	public static Vector2d operator /(Vector2d a, double b)
	{
		return new Vector2d(a.x / b, a.y / b);
	}
	
	public static Vector2d operator +(Vector2d a, Vector2d b)
	{
		return new Vector2d(a.x + b.x, a.y + b.y);
	}
	
	public static Vector2d operator -(Vector2d a, Vector2d b)
	{
		return new Vector2d(a.x - b.x, a.y - b.y);
	}
}

public static class Extensions {

	public static Vector2 Abs(this Vector2 vector) {
		for (int i = 0; i < 2; ++i) vector[i] = Mathf.Abs(vector[i]);
		return vector;
	}
	
	public static Vector2 DividedBy(this Vector2 vector, Vector2 divisor) {
		for (int i = 0; i < 2; ++i) vector[i] /= divisor[i];
		return vector;
	}
	
	public static Vector2 IntersectionWithRayFromCenter(Vector2d boxMin, Vector2d boxMax, Vector2d pointOnRay) {	
		var boxCenter = (boxMin + boxMax)/2;
	
		var pointOnRayLocal = pointOnRay - boxCenter;
		var edgeToRayRatios = (boxMax - boxCenter).DividedBy(pointOnRayLocal.Abs());
		
		if (edgeToRayRatios.x < edgeToRayRatios.y) {
			return new Vector2((float)(pointOnRayLocal.x > 0 ? boxMax.x : boxMin.x), 
			            (float)(pointOnRayLocal.y * edgeToRayRatios.x + boxCenter.y));
		} else {
			return new Vector2((float)(pointOnRayLocal.x * edgeToRayRatios.y + boxCenter.x), 
	            (float)(pointOnRayLocal.y > 0 ? boxMax.y : boxMin.y));
        }
	}
	
	public static float AngleInRadians(this Vector2 v) {
		return Mathf.Atan2(v.y, v.x);	
	}
	
//	public static float Angle(this Vector3 v) {
//		return Mathf.Atan2 (v.y, v.x);	
//	}
	
	public static Vector2 Rotated(this Vector2 v, float radians) {
		var angle = v.AngleInRadians() + radians;
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
	
	public static float Distance(this Transform t, Vector2 other) {
		return Vector2.Distance(t.position, other);
	}
}
