using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPageController : ScrollRect
{
	[HideInInspector]
	public UIPageView pageView;

	private bool isMoving = false;

	private void Update() 
	{
		// if (isMoving)
		// {
		// }
		
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);

		// Debug.Log("xx-- UIPageController.OnEndDrag > " + pageView.contentRT.anchoredPosition);
		// float px = Mathf.Abs(pageView.contentRT.anchoredPosition.x) / pageView.contentRT.sizeDelta.x;
		// Debug.Log("xx-- px > " + px);
		// isMoving = true;
	}
}
