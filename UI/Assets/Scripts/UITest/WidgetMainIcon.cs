// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class WidgetMainIcon : UIWidget
// {
// 	public Action<WidgetMainIcon> onClick = null;

// 	public enum Type
// 	{
// 		None,
// 		Bag,
// 		Shop,
// 	}

// 	public Type type;

// 	public void OnClicked()
// 	{
// 		Debug.Log("xx-- WidgetMainIcon clicked");
// 		if (type == Type.Bag)
// 		{
// 			UIManager.Instance.OpenMenu("MenuBag");
// 		}
// 		else if (type == Type.Shop)
// 		{
// 			UIManager.Instance.OpenMenu("MenuShop");
// 		}
// 	}
// }
