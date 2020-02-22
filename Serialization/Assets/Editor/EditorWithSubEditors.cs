using UnityEngine;
using UnityEditor;

public abstract class EditorWithSubEditors<TEditor, TTarget> : Editor
		where TEditor : Editor
		where TTarget : Object
{
	protected TEditor[] subEditors;

	protected void CheckAndCreateSubEditors(TTarget[] subEditorTargets)
	{
		if (subEditors != null && subEditors.Length == subEditorTargets.Length)
			return;

		CleanupEditors();

		Debug.Log("xx-- subEditor len > " + subEditorTargets.Length);
		subEditors = new TEditor[subEditorTargets.Length];
		for (int i = 0; i < subEditors.Length; ++i)
		{
			subEditors[i] = CreateEditor(subEditorTargets[i]) as TEditor;
			if (subEditors[i] == null)
			{
				Debug.Log("xx-- subEditor is null > " + subEditorTargets[i]);
				continue;
			}
			SubEditorSetup(subEditors[i]);
		}
	}

	protected void CleanupEditors()
	{
		Debug.Log("xx-- cleanup editors");

		if (subEditors == null)
			return;

		for (int i = 0; i < subEditors.Length; ++i)
		{
			// In editor, Destroy may not be called
			DestroyImmediate(subEditors[i]);
		}

		subEditors = null;
	}

	protected abstract void SubEditorSetup(TEditor editor);
}