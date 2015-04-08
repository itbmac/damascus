using UnityEngine;
using System.Collections;

public class VisionAdapter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float width = 5;
		float height = 5;
	
		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = new Vector3[] {
			new Vector3(-width, -height, 0.01f),
			new Vector3(width, -height, 0.01f),
			new Vector3(width, height, 0.01f),
			new Vector3(-width, height, 0.01f)
		};
		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			new Vector2 (1, 0)
		};
		m.triangles = new int[] { 0, 1, 2, 0, 2, 3};
		m.normals = new Vector3[] {
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1),
			new Vector3(0, 0, -1)
		};
		
		GetComponent<MeshFilter>().mesh = m;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
