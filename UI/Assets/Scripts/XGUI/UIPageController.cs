using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIPageController : ScrollRect
{
	public override void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("xx-- UIPageController.OnEndDrag");
	}
}
