using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 所有控件扩展的基类
public class UIWidget : MonoBehaviour
{
	protected List<UIWidget> children = new List<UIWidget>();

	// public GameObject container = null;
	public virtual void AddChild(UIWidget widget)
	{
	}
}
