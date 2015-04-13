using UnityEngine;
using System.Collections;

public class SpawnCasters : MonoBehaviour {

	GameObject []casters;
	GameObject line;


	void Start () {
		casters = new GameObject[7];


		casters[0] = GameObject.Find("square");
		line = GameObject.Find("line");

		for (int i =1 ; i<casters.Length; i++){
			casters[i] = Instantiate(casters[0]) as GameObject;
			casters[i].transform.position = new Vector3(Random.Range(-1f,1f) * 35f, Random.Range(-1f,1f) * 35f, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {

		for (int i =0 ; i<casters.Length; i++){
			Vector3 cPos = casters[i].transform.position;

			if(cPos.y < -30){
				cPos.y = 35;
				cPos.x = Random.Range(-1f,1f) * 15f;
				if(cPos.x < 0){
					cPos.x -= 12;
				}
				if(cPos.x > 0){
					cPos.x += 12;
				}
			}else{
				cPos.y -= 0.9f;
			}


			casters[i].transform.position = cPos;
		}


		Vector3 lPos = line.transform.position;
		if(lPos.y < -30){
			lPos.y = 40;
		}else{
			lPos.y -= 0.9f;
		}
		line.transform.position = lPos;

	}
}
