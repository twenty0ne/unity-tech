using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMain : UIMenu
{
	// public Transform tfIconBag;

	private void Start()
	{
	}

	// public void OnClickWidgetMainIcon(WidgetMainIcon widget)
	// {
	// 	// UIManager.Instance.OpenMenu("MenuLogin");
	// 	// if (widget)
	// }
	private void Update() 
	{
		// if (Input.GetKeyDown(KeyCode.Space))
		// {
		// 	TestMove();
		// }
	}

	// public void TestMove()
	// {
	// 	Vector3 pos = tfIconBag.position;
	// 	pos.y += 0.01f;
	// 	tfIconBag.position = pos;
	// }

	public void OnClickBtnBag()
	{
		Debug.Log("OnClickBtnBag");
		UIManager.Instance.OpenMenu("MenuBag");
	}

	public void OnClickBtnShop()
	{
		Debug.Log("OnClickBtnShop");
	}
}
