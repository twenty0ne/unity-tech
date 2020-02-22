using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyObject))]
public class MyObjectEditor : Editor
{
	private MyObject myObject;
	private SerializedProperty myItemProperty;

	// protected override void SubEditorSetup(MyItemEditor editor)
	// {
	// 	// editor.reactionsProperty = reactionsProperty;
	// }

	// private void OnDisable() 
	// {
	// 	CleanupEditors();
	// }

	private void OnEnable() 
	{
		myObject = (MyObject)target;

		myItemProperty = serializedObject.FindProperty("myItem");
	}

	public override void OnInspectorGUI()
	{
		// DrawDefaultInspector();
		// return;

		serializedObject.Update();

		if (myObject.myItem != null)
		{
			// AssetDatabase.StartAssetEditing();

			myObject.myItem.id = EditorGUILayout.IntField(myObject.myItem.id);
			// myObject.myItem.name = EditorGUILayout.TextField(myObject.myItem.name);

			EditorUtility.SetDirty(myObject.myItem);
			EditorUtility.SetDirty(myObject);
			// AssetDatabase.StartAssetEditing
			// AssetDatabase.SaveAssets();

			// AssetDatabase.StopAssetEditing();
			if (GUILayout.Button("Save", GUILayout.Width(70f)))
			{
				AssetDatabase.SaveAssets();
			}
		}
		else
		{
			if (GUILayout.Button("Add", GUILayout.Width(70f)))
			{
				// AssetDatabase.StartAssetEditing();

				var myItem = (MyItem)ScriptableObject.CreateInstance(typeof(MyItem));
				// AssetDatabase.CreateAsset(myItem, "Assets/Temp/kk.asset");
				// AssetDatabase.SaveAssets();
				// myObject.myItem = myItem;

				// AssetDatabase.AddObjectToAsset(myItem, myObject);
				myItem.hideFlags = HideFlags.None;
				myItem.name = "kk";

				// AssetDatabase.AddObjectToAsset(myItem, myObject);
				myObject.myItem = myItem;
				// AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(myItem));

				EditorUtility.SetDirty(myObject.myItem);
				EditorUtility.SetDirty(myObject);
				// AssetDatabase.StopAssetEditing();

				AssetDatabase.SaveAssets();
			}

			if (GUILayout.Button("Attach", GUILayout.Width(70f)))
			{
				var obj = AssetDatabase.LoadAssetAtPath<MyItem>("Assets/Temp/kk.asset");
				if (obj == null)
					Debug.Log("xx-- no kk.asset");
				obj.hideFlags = HideFlags.None;

				AssetDatabase.AddObjectToAsset(obj, myObject);
				// EditorUtility.SetDirty(myObject);
				// AssetDatabase.SaveAssets();
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
				EditorUtility.SetDirty(myObject);
				AssetDatabase.SaveAssets();
			}

			EditorGUILayout.PropertyField(myItemProperty);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
