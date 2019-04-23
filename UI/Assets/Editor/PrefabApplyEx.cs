using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[InitializeOnLoad]
public class PrefabApplyEx 
{
	static PrefabApplyEx()
	{
		UnityEditor.PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdate;
	}

	static void OnPrefabInstanceUpdate(GameObject obj)
	{
		Debug.Log("PrefabApplyEx on prefab > " + obj.name);
		CheckButtonClickMissing(obj);
	}

	static void CheckButtonClickMissing(GameObject obj)
	{
		foreach (var btn in obj.GetComponentsInChildren<Button>())
		{
			if (btn.onClick == null || btn.onClick.GetPersistentEventCount() == 0)
			{
				Debug.LogWarning(string.Format("Button {0} miss onClick callback.", btn.gameObject.FullName()));
				continue;
			}
			
			for (int i = 0; i < btn.onClick.GetPersistentEventCount(); ++i)
			{
				if (btn.onClick.GetPersistentTarget(i) == null || string.IsNullOrEmpty(btn.onClick.GetPersistentMethodName(i)))
				{
					Debug.LogWarning(string.Format("Button {0} miss onClick callback.", btn.gameObject.FullName()));
				}
			}
		}
	}
}