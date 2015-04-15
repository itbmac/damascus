using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]

public class TagLayerClass{

	static TagLayerClass()
	{
		createLayer();
	}
	
	static void createLayer(){
		SerializedObject SerializedObjectTagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		SerializedProperty it = SerializedObjectTagManager.GetIterator();

		bool showChildren = true;

#if UNITY_5

		while(it.NextVisible (showChildren)){

			if(it.displayName == "Element 8"){
				it.stringValue = "ShadowLayer";
			}

		}



#else
		while(it.NextVisible (showChildren)){
			
			if(it.name == "User Layer 8"){
				it.stringValue = "ShadowLayer";
			}
			
		}
		//mmmm
#endif

		SerializedObjectTagManager.ApplyModifiedProperties();

	}
}
