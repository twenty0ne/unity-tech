using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 所有控件扩展的基类
[RequireComponent(typeof(RectTransform))]
public class UIWidget : MonoBehaviour
{
	protected List<UIWidget> children = new List<UIWidget>();

	protected RectTransform _rt = null;

	private void Awake()
	{
		_rt = GetComponent<RectTransform>();
	}

	// public GameObject container = null;
	public virtual void AddChild(UIWidget widget)
	{
	}

	// 隔绝下层 Update，更好的控制
	public virtual void Tick(float dt)
	{

	}
}
