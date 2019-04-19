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
		AssetManager.LoadGameObject()

		yield return null;
	}

	public void OnClickWidgetMainIcon(WidgetMainIcon widget)
	{
		// UIManager.Instance.OpenMenu("MenuLogin");
		// if (widget)
	}
}
