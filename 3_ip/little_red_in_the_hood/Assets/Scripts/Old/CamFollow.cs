using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {
	public float cameraSize = 12.0f;
	public float cameraSizeTarget = 12.0f;
	public float cameraSizeStart = 8.0f;
	public float cameraSizeZoomed = 12.0f;
	public float cameraSizeTransitionTimeStandard = 1.3f;
	public float cameraSizeTransitionTimeFactorCurrent = 1.0f;
	private float cameraSizeTransitionTimeStart;

	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay = 0.002f;
	public float shake_intensity = 0f;
	public float SHAKE_INTENSITY_0 = 0.015f;
	public float SHAKE_INTENSITY_1 = 0.025f;
	public float SHAKE_INTENSITY_2 = 0.035f;
	public float SHAKE_INTENSITY_3 = 0.050f;
	public float SHAKE_INTENSITY_4 = 0.075f;
	private Camera cam;

	public Transform target;

	void Start() {
		cam = this.GetComponent<Camera>();
		cameraSize = cam.orthographicSize;
		cameraSizeTarget = cameraSize;
		cameraSize = cameraSizeStart;
		shake_intensity = 0.0f;
	}

	void Update()
	{
		if (target)
		{

			if (cameraSize != cameraSizeTarget) {
				if (cameraSize < cameraSizeTarget) {
					cameraSize = Mathf.Min(cameraSizeTarget, cameraSize + (Time.time - cameraSizeTransitionTimeStart)/(cameraSizeTransitionTimeStandard * cameraSizeTransitionTimeFactorCurrent));
				}
				else {
					cameraSize = Mathf.Max(cameraSizeTarget, cameraSize - (Time.time - cameraSizeTransitionTimeStart)/(cameraSizeTransitionTimeStandard * cameraSizeTransitionTimeFactorCurrent));
				}
				cam.orthographicSize = cameraSize;
			}

			if (shake_intensity > 0) {
				Vector3 shake = Random.insideUnitSphere * shake_intensity;
				shake.z = 0.0f;
				transform.position = originPosition + shake;
				transform.rotation = new Quaternion(
					originRotation.x + Random.Range (-shake_intensity,shake_intensity) * .2f,
					originRotation.y + Random.Range (-shake_intensity,shake_intensity) * .2f,
					originRotation.z, // + Random.Range (-shake_intensity,shake_intensity) * .2f,
					originRotation.w); // + Random.Range (-shake_intensity,shake_intensity) * .2f);
				shake_intensity -= shake_decay;
			}

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

	public void ChangeSize(float newCameraSize, float transitionSpeed = 1.0f) {
		cameraSizeTransitionTimeStart = Time.time;
		cameraSizeTarget = newCameraSize;
		cameraSizeTransitionTimeFactorCurrent = transitionSpeed;
	}
	
	public void Shake(float shakeIntensity, float shakeDecay = 0.002f){
		originPosition = transform.position;
		originRotation = transform.rotation;
		shake_intensity = shakeIntensity;
		shake_decay = shakeDecay;
	}
}
