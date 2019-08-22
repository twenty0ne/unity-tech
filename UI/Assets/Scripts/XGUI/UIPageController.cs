using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPageController : ScrollRect
{
	private UIPageView _pageView;

	private bool isMoving = false;
	private RectTransform _pageViewRTF = null;

	// protected override void Start() 
	// {
	// 	base.Start();

	// 	// pageView = content.GetComponent<UIPageView>();
	// 	// Debug.Assert(pageView);
	// }

	public UIPageView PageView 
	{
		get { return _pageView; }
		set 
		{
			_pageView = value;
			_pageViewRTF = _pageView.GetComponent<RectTransform>();
		}
	}

	private void Update() 
	{
		if (_pageView == null)
			return;

		 // _pageViewRTF.anchoredPosition / this.
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

	// private void MoveToIndex()
}
