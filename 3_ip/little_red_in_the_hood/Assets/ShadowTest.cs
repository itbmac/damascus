using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(PolygonCollider2D))]
public class ShadowTest : MonoBehaviour {

	public float UpdateTime = 0.01f;	
	public float MaxDistance = 5.0f;
	public float DirectionAngle;
	public float AngleSize = Mathf.PI/2;	
	public float CastDistanceMultiplier = 3;
	
	Mesh mesh;
	float nextUpdate;
	int layerMask;
	
	// Use this for initialization
	void Start () {		
		mesh = new Mesh();
		mesh.name = "ScriptedMesh";		
		GetComponent<MeshFilter>().mesh = mesh;	
		
		layerMask = LayerMask.GetMask("Obstacle");
	}	
	
	Vector2 Raycast(Vector2 dir) {
		var result = Physics2D.Raycast(transform.position, dir.normalized, MaxDistance, LayerMask.GetMask("Obstacle"));
		
		if (result)
			return result.point - (Vector2)transform.position;
		else
			return dir.normalized * MaxDistance;
	}	
	
	bool IsBetween(Vector2 dir, float a, float b) {
		// a and b are angles, 0 <= a, b <= 2pi
		
		float angle = dir.Angle();
		
		if (a <= b)
			return a <= angle && angle <= b;
		else
			return a <= angle || angle <= b;
	
	}
	
	float AngleDiff(float a, float b) {
		float diff = Mathf.Repeat((a - b) + Mathf.PI * 4, Mathf.PI * 2);
		if (diff > Mathf.PI/2)
			diff -= Mathf.PI * 2;
			
		return diff;
	}
	
	Vector2 ConstrainPointToLine(Vector2 center, Vector2 lineP1, Vector2 lineP2, Vector2 point) {		
		var r_p = center;
		var r_d = point - center;
		var s_p = lineP1;
		var s_d = lineP2 - lineP1;
		
		var intersect = Extensions.RayLineIntersection(r_p, r_d, s_p, s_d);
		
		// TODO: optimize out the sqrt using t?
		if (Vector2.Distance(center, intersect) < Vector2.Distance(center, point)) {
			return intersect;
		} else {
			return point;		
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time < nextUpdate)
			return;
		else
			nextUpdate = Time.time + UpdateTime;
			
		var l = new List<Vector2>();
		
		var hit = Physics2D.OverlapCircleAll(transform.position, MaxDistance, layerMask);
		
		// TODO: clamp to line intersection
		// TODO: cull here by angle
		foreach (var col in hit) {
			if (!(col is PolygonCollider2D)) continue;
			
			var opc = (PolygonCollider2D)col;
			for (int i = 0; i < opc.pathCount; i++) {
				l.AddRange(opc.GetPath(i).Select(x => x + (Vector2)col.transform.position).
				           Where(x => Vector2.Distance(transform.position, x) < MaxDistance*CastDistanceMultiplier));
			}
		}
//		Debug.Log (l.Count);
//		foreach (var x in l) {
//			Debug.DrawLine(transform.position, x, Color.green, UpdateTime);
////			print ( (x - (Vector2)transform.position).Angle());
//		}

		var dir = Extensions.UnitVectorForAngle(DirectionAngle);
		
		var lower = dir.Rotated(AngleSize/2) * MaxDistance;
		var upper = dir.Rotated(-AngleSize/2) * MaxDistance;		
		
		mesh.Clear();
		mesh.vertices = l
			.Select(x => x - (Vector2)transform.position)
			.Where(v => (Vector2.Angle(dir, v.normalized) * Mathf.Deg2Rad) <= AngleSize/2)
			.SelectMany(v => new Vector2[] {v.Rotated(-0.1f), v, v.Rotated(0.1f)})
			.Concat(new Vector2[] {lower, upper})
			.OrderBy(v => AngleDiff(v.Angle(), DirectionAngle) )
			.Select(x => Raycast(x))	
//			.Select(x => ConstrainPointToLine(Vector2.zero, lower, upper, x))		
			.Select(x => (Vector3)x)
			.Prepend(Vector3.zero)		
			.ToArray();
			
//		foreach (var x in mesh.vertices) {
//			Debug.DrawLine(transform.position, transform.position + x, Color.red, UpdateTime);
//		}
				
//		for (int i = 1; i < mesh.vertices.Length; i++) {
//			if (i == 0)
//				Debug.DrawLine(
//					transform.position + mesh.vertices.First(),
//					transform.position + mesh.vertices.Last(),
//					Color.blue,
//					UpdateTime
//				);
//			else
//				Debug.DrawLine(
//					transform.position + mesh.vertices[i - 1],
//					transform.position + mesh.vertices[i],
//					Color.blue,
//					UpdateTime
//				);
//		}
		
//		Debug.Log (mesh.vertices.Length);
		
		mesh.uv = mesh.vertices.Select(x => Vector2.zero).ToArray();
		
		if (mesh.vertices.Length < 3)
			mesh.triangles = new int[] {};
		else {
			mesh.triangles = Enumerable.Range(2, mesh.vertices.Length - 2).SelectMany(x => new int[] {0, x - 1, x})
//				.Concat(new int[] {0, 1, mesh.vertices.Length - 1})
				.ToArray();
		}
		
		
		mesh.normals = mesh.vertices.Select(x => new Vector3(0, 0, -1)).ToArray();
		
		var pc = GetComponent<PolygonCollider2D>();
		pc.SetPath(0, mesh.vertices.Select(x => (Vector2)x).ToArray());
		
	}
}
