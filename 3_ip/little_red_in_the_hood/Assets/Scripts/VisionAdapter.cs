using UnityEngine;
using System.Collections;
using System.Linq;

public class VisionAdapter : MonoBehaviour {

	Mesh mesh;

	// Use this for initialization
	void Start () {
		
	
		mesh = new Mesh();
		mesh.name = "ScriptedMesh";
		
		
		GetComponent<MeshFilter>().mesh = mesh;	
	}
	
	float nextUpdate;
	public float UpdateTime = 1.0f;
	public float Speed = 1.0f;
	
	Vector3 RandomOffset(float t) {
		t *= Speed;
		return new Vector3(Mathf.Sin(t), Mathf.Cos (t), 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time < nextUpdate)
			return;
		else
			nextUpdate = Time.time + UpdateTime;
			
		float width = 5;
		float height = 5;
		
		mesh.vertices = new Vector3[] {
			new Vector3(-width, -height, 0.01f) + 2 * RandomOffset(Time.time + 2f),
			new Vector3(width, -height, 0.01f) + 3 * RandomOffset(Time.time + 1.5f),
			new Vector3(width, height, 0.01f) + .5f * RandomOffset(Time.time + 1f),
			new Vector3(-width, height, 0.01f) + 1.2f * RandomOffset(Time.time + 0.5f)
		};
		mesh.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			new Vector2 (1, 0)
		};
		mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3};
		mesh.normals = new Vector3[] {
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1)
		};
	
		var pc = GetComponent<PolygonCollider2D>();
		pc.SetPath(0, mesh.vertices.Select(x => (Vector2)x).ToArray());
	
	}
}
