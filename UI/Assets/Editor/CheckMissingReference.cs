// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using System.Linq;

// public class CheckMissingReference : Editor
// {
// 	[MenuItem("Tools/CheckMissingReference/Select")]
// 	public static void CheckSelect()
// 	{
// 		string strMsg = string.Empty;

// 		GameObject[] objs = Selection.gameObjects;
// 				if (objs == null)
// 						return;

// 		foreach (var obj in objs)
// 				{
// 						strMsg += CheckGameObject(obj);
// 		}

// 		if (!string.IsNullOrEmpty(strMsg))
// 		{
// 						EditorUtility.DisplayDialog("WARNING: Missing Reference", strMsg, "OK");
// 		}
// 	}

// 		[MenuItem("Tools/CheckMissingReference/CurrentScene")]
// 		public static void CheckCurrentScene()
// 		{
// 				string strMsg = string.Empty;

// 				var objs = Object.FindObjectsOfType<GameObject>();
// 				foreach (var obj in objs)
// 				{
// 						strMsg += CheckGameObject(obj);
// 				}

// 				if (!string.IsNullOrEmpty(strMsg))
// 				{
// 						EditorUtility.DisplayDialog("WARNING: Missing Reference", strMsg, "OK");
// 				}
// 		}

// 		[MenuItem("Tools/CheckMissingReference/AllScenes")]
// 		public static void CheckAllScenes()
// 		{
// 				string strMsg = string.Empty;

// 				foreach (var scene in EditorBuildSettings.scenes)
// 				{
// 						EditorSceneManager.OpenScene(scene.path);

// 						var objs = Object.FindObjectsOfType<GameObject>();
// 						foreach (var obj in objs)
// 						{
// 								strMsg += CheckGameObject(obj);
// 						}
// 				}

// 				if (!string.IsNullOrEmpty(strMsg))
// 				{
// 						EditorUtility.DisplayDialog("WARNING: Missing Reference", strMsg, "OK");
// 				}
// 		}

// 		[MenuItem("Tools/CheckMissingReference/Assets")]
// 		public static void CheckAssets()
// 		{
// 				string strMsg = string.Empty;

// 				var allAssetsPath = AssetDatabase.GetAllAssetPaths();
// 				for (int i = 0; i < allAssetsPath.Length; ++i)
// 				{
// 						string path = allAssetsPath[i];
// 						GameObject obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
// 						if (obj == null)
// 								continue;

// 						strMsg += CheckGameObject(obj);
// 				}

// 				if (!string.IsNullOrEmpty(strMsg))
// 				{
// 						EditorUtility.DisplayDialog("WARNING: Missing Reference", strMsg, "OK");
// 				}
// 		}
				
// 		[InitializeOnLoadMethod]
// 		static void StartInitializeOnLoadMethod () 
// 		{
// 				PrefabUtility.prefabInstanceUpdated = delegate(GameObject obj) {
// 								//Debug.Log(AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(obj)));
// 						string strMsg = CheckGameObject(obj);
// 						if (string.IsNullOrEmpty(strMsg) == false)
// 						{
// 								EditorUtility.DisplayDialog("WARNING: Missing Reference", strMsg, "OK");
// 						}
// 				};
// 		}

// 		static string CheckGameObject(GameObject obj)
// 		{
// 				if (obj == null)
// 						return string.Empty;

// 				string str = string.Empty;

// 				Component[] cmps = obj.GetComponentsInChildren<MonoBehaviour>();
// 				for (int i = 0; i < cmps.Length; ++i)
// 				{
// 						Component cmp = cmps[i];
// 						if (cmp == null)
// 								continue;
// 						// TODO
// 						// only check UINode
// 						if ((cmp is UINode) == false)
// 								continue;

// 						SerializedObject sobj = new SerializedObject(cmp);
// 						SerializedProperty sprop = sobj.GetIterator();
// 						while(sprop.NextVisible(true))
// 						{
// 								if (sprop.propertyType != SerializedPropertyType.ObjectReference)
// 										continue;
// 								if (sprop.objectReferenceValue != null)
// 										continue;
// 								//                    if (sprop.objectReferenceInstanceIDValue == 0)
// 								//                        continue;

// 								str += obj.name + "-" + cmp.GetType().ToString() + "-" + sprop.propertyPath + "\n";
// 						}
// 				}

// 				return str;
// 		}
// }
