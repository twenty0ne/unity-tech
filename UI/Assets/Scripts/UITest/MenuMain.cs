using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : UIMenu
{
	private void Start()
	{
		StartCoroutine(InitWidgets());
	}

	private IEnumerator InitWidgets()
	{
		GameObject objWidget = AssetManager.LoadGameObject(AssetDefine.PATH_PREFAB_UI_WIDGET	+ "WidgetMainIcon");
		Debug.Assert(objWidget != null, "CHECK");

		for (int i = 0; i < )


		yield return null;
	}

	public void OnClickWidgetMainIcon(WidgetMainIcon widget)
	{
		// UIManager.Instance.OpenMenu("MenuLogin");
		// if (widget)
	}
}
