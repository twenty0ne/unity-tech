using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPageView : UIWidget
{
	public UIPageViewIndicator indicator;
	public ScrollRect scrollRect;

	private void Awake()
	{
		scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
	}

	public override void AddChild(UIWidget widget)
	{
		widget.transform.SetParent(transform, false);
	}

	private void OnScrollValueChanged(Vector2 vec2)
	{
		
	}
}
