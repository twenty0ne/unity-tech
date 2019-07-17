using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPageView : UIWidget
{
	public UIPageViewIndicator indicator;
	public UIPageController controller;

	private RectTransform contentRT;

	// public List<UIWidget> children = new List<UIWidget>();

	private void Awake()
	{
		contentRT = GetComponent<RectTransform>();

		// scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
		// scrollRect.
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

	// public void OnBeginDrag(PointerEventData eventData)
	// {
	// 	Debug.Log("xx-- UIPageView.OnBeginDrag > " + eventData.ToString());
	// 	return;
	// }

	// public void OnDrag(PointerEventData eventData)
	// {
	// 	Debug.Log("xx-- UIPageView.OnDrag > " + eventData.ToString());
	// }

	// public void OnEndDrag(PointerEventData eventData)
	// {
	// 	Debug.Log("xx-- UIPageView.OnEndDrag > " + eventData.ToString());
	// }

	// public void OnScrollToItem(int idx)
	// {

	// }
}
