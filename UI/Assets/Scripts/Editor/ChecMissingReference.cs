using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CheckMissingReference : Editor
{
	[MenuItem("Tool/UI/CheckMissingReference")]
	public static void Process()
	{
		string strMsg = string.Empty;

		GameObject[] objs = Selection.gameObjects;
		foreach (var obj in objs)
		{
			MonoBehaviour[] mbs = obj.GetComponentsInChildren<MonoBehaviour>(true);
			for (int i = 0; i < mbs.Length; ++i)
			{
				MonoBehaviour mb = mbs[i];
				if (mb == null)
					continue;

				SerializedObject sobj = new SerializedObject(mb);
				SerializedProperty sprop = sobj.GetIterator();
				while(sprop.NextVisible(true))
				{
					if (sprop.propertyType == SerializedPropertyType.ObjectReference &&
						sprop.objectReferenceValue == null &&
						sprop.objectReferenceInstanceIDValue != 0)
					{
						strMsg += obj.name + "-" + mb.GetType().ToString() + "-" + sprop.propertyPath + "\n";
					}
				}
			}
		}

		if (!string.IsNullOrEmpty(strMsg))
		{
			EditorUtility.DisplayDialog("Missing Reference", strMsg, "OK");
		}
	}
}
