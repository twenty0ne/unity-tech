using System.Text;
using UnityEngine;

public static class UIUtils
{
	public static T CreateWidget<T>(string widgetName)
	{
		GameObject prefab = AssetManager.LoadGameObject(AssetDefine.PATH_PREFAB_UI_WIDGET + widgetName);
		GameObject obj = Object.Instantiate(prefab);
		T widget = obj.GetComponent<T>();
		Debug.Assert(widget != null, "CHECK");
		return widget;
	}

	public static string FullName(this GameObject obj)
	{
		StringBuilder sb = new StringBuilder();

		GameObject pobj = obj;
		while (pobj.transform.parent != null)
		{
			sb.Insert(0, pobj.name + " > ");
		}

		return sb.ToString();
	}
}