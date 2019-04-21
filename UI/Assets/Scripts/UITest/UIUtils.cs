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
}