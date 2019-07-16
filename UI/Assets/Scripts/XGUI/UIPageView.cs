using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPageView : UIWidget, IPointerUpHandler
{
	public UIPageViewIndicator indicator;
	public ScrollRect scrollRect;

	private RectTransform contentRT;

	// public List<UIWidget> children = new List<UIWidget>();

	private void Awake()
	{
		contentRT = GetComponent<RectTransform>();

		scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
	}

	public override void AddChild(UIWidget widget)
	{
		widget.transform.SetParent(transform, false);
		children.Add(widget);
	}

	private void OnScrollValueChanged(Vector2 val)
	{
		// Debug.Log("xx-- UIPageView.OnScrollValueChanged > " + vec2.x + " - " + vec2.x * contentRT.sizeDelta.x);
		float vx = val.x;

	}

	// private void OnMouseUp() 
	// {
	// 	Debug.Log("xx-- OnMouseUp");
	// }
	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log("xx-- OnPointerUp");
	}
}
