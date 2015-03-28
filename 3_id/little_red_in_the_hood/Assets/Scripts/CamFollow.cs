using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {
	public float cameraSize = 12.0f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	void Start() {
		Camera cam = this.GetComponent<Camera>();
		cameraSize = cam.orthographicSize;
	}

	void Update()
	{
		if (target)
		{
			float player_x = target.position.x;
			float player_y = target.position.y;

			float rounded_x = RoundToNearestPixel(player_x);
			float rounded_y = RoundToNearestPixel(player_y);
			
			Vector3 new_pos = new Vector3(rounded_x, rounded_y, -1 * cameraSize); // this is 2d, so my camera is that far from the screen.
			transform.position = new_pos;
		}
	}
	public float pixelToUnits = 100f;
	
	public float RoundToNearestPixel(float unityUnits)
	{
		float valueInPixels = unityUnits * pixelToUnits;
		valueInPixels = Mathf.Round(valueInPixels);
		float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
		return roundedUnityUnits;
	}
}
