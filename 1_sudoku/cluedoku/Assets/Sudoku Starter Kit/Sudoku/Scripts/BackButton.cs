using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour
{
		public int levelNo;			// level number
	
		// Update is called once per frame
		void Update ()
		{
			if(Input.GetKeyUp(KeyCode.Escape)){			// on escape key
				Application.LoadLevel(levelNo);						// load level
			}	
		}
}

