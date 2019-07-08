using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListItem : UIPanel
{
	// NOTE:
	// 不要直接引用 TestListView, 记住 UIPanel 是可能在多个地方使用
	public System.Action<TestListItem> onItemClick = null;

	// public Item 
	private void Awake() 
	{
		// Debug.Log("xx-- item awake");
	}

	void Start()
	{
		// Debug.Log("xx-- item start");
	}

	public void OnClick()
	{
		Debug.Log("xx-- on click item");
		// TODO:
		// 消息需要传递到上层 TestListView
		if (onItemClick != null)
			onItemClick(this);
	}
}
