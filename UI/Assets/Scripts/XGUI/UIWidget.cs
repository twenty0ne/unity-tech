using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 所有控件扩展的基类
[RequireComponent(typeof(RectTransform))]
public class UIWidget : MonoBehaviour, IEventListener
{
	protected List<UIWidget> children = new List<UIWidget>();

	protected RectTransform _rt = null;

	private void Awake()
	{
		_rt = GetComponent<RectTransform>();
	}

	protected virtual void OnCreate()
	{

	}

	protected virtual void OnClean()
	{
		
	}

	public virtual void Active()
	{
		for (int i = 0; i < children.Count; ++i)
		{
			children[i].Active();
		}

		OnActive();
	}

	public virtual void Deactive()
	{
		for (int i = 0; i < children.Count; ++i)
		{
			children[i].Deactive();
		}

		OnDeactive();
	}

	protected virtual void OnActive()
	{

	}

	protected virtual void OnDeactive()
	{

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
