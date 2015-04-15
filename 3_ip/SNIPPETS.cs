var so = new SerializedObject(this);
var p = so.FindProperty("ViceCopMode");
p.boolValue = GetComponent<MoveBetween>().ViceCopMode;
var p2 = so.FindProperty("WPoints");
p2.ClearArray();

var L = GetComponent<MoveBetween>().WPoints;
p2.arraySize = L.Count();

for (int i = 0; i < p2.arraySize; i++)
	p2.GetArrayElementAtIndex(i).vector2Value = L[i];


so.ApplyModifiedProperties();

DestroyImmediate(GetComponent<MoveBetween>());

