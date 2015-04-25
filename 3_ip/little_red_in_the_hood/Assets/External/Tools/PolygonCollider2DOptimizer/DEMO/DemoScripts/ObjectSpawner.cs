using UnityEngine;
using System.Collections;

namespace unitycodercom_extras
{

	public class ObjectSpawner : MonoBehaviour {

		public Transform[] prefabs;

		public GUIText guiCounter;
		int counter = 0;

		public float fireRate = 0.25F;
		private float nextFire = 0.0F;

		void Start () {
		
		}
		

		void Update () 
		{
			// spawn on clickdown
			if (Input.GetMouseButton(0) && Time.time > nextFire) 
			{
				nextFire = Time.time + fireRate;
				Instantiate(prefabs[Random.Range(0,prefabs.Length)],transform.position, Quaternion.identity);

				float posX = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f,0.9f),0,10)).x;
				transform.position = new Vector3(posX, transform.position.y ,transform.position.z);
				counter++;
				guiCounter.text = ""+counter;

			}
		}
	}
}