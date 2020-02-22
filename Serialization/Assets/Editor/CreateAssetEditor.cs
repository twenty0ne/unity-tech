using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAssetEditor
{
	[MenuItem("Test/TestAsset1")]
	public static void CreateAsset()
	{
		TestAsset1 ta = ScriptableObject.CreateInstance<TestAsset1>();
		AssetDatabase.CreateAsset(ta, "Assets/EditorCreateTestAsset1.asset");
		ta.id = 100;
		AssetDatabase.SaveAssets();
	}
}
