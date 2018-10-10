using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using System.IO;

public class BuildProcessor : IPreprocessBuild
{
	public int callbackOrder { get { return 0; } }
	public void OnPreprocessBuild(BuildTarget target, string path)
	{
		string coreAarPath = Application.dataPath + "/../../sdk/android/core/build/outputs/aar/core-debug.aar";
		SafeCopy(coreAarPath, Application.dataPath + "/XSdk/Plugins/Android/libs/core.aar");

		string pluginAarPath = Application.dataPath + "/../../sdk/android/plugin/build/outputs/aar/plugin-debug.aar";
		SafeCopy(pluginAarPath, Application.dataPath + "/XSdk/Plugins/Android/libs/plugin.aar");

		AssetDatabase.Refresh();
	}

	private void SafeCopy(string fromPath, string toPath)
	{
		if (File.Exists(fromPath) == false)
		{
			Debug.LogWarning("BuildProcessor.SafeCopy > failed to find fromPath " + fromPath);
			return;
		}

		if (File.Exists(toPath))
		{
			File.SetAttributes(toPath, FileAttributes.Normal);
			File.Delete(toPath);
		}

		File.Copy(fromPath, toPath);
	}
}
