using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPageView : UIWidget
{
	public UIPageViewIndicator indicator;
	public UIPageController controller;

	[HideInInspector]
	public RectTransform contentRT;

	// public List<UIWidget> children = new List<UIWidget>();

	private void Awake()
	{
		contentRT = GetComponent<RectTransform>();

		// scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
		// scrollRect.
		controller.PageView = this;
	}

	public override void AddChild(UIWidget widget)
	{
		widget.transform.SetParent(transform, false);
		children.Add(widget);
	}

	private void OnScrollValueChanged(Vector2 val)
	{
		// Debug.Log("xx-- UIPageView.OnScrollValueChanged > " + val.x + " - " + val.x * contentRT.sizeDelta.x);
		float vx = val.x;

	}
}
