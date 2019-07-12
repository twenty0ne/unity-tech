using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class UIListView : UIWidget
{
	public LayoutGroup layoutGroup;
	public GameObject pfbItem;
	public int itemCount = 0;

	public override void AddChild(UIWidget widget)
	{
		Debug.Assert(widget != null, "CHECK");
		widget.transform.SetParent(transform, false);
		children.Add(widget);

		// var rt = widget.GetComponent<RectTransform>();
		// float width = rt.sizeDelta.x;
		// float height = rt.sizeDelta.y;

		// Resize(width, height);
	}

	private void Resize(float itemWidth, float itemHeight)
	{
		// layoutGroup.
	}
}
