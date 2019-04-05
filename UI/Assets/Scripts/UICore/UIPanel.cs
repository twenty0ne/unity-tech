using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : UINode 
{
	// 传递进来的参数
	public Dictionary<string, object> param;

	// TODO:
	// more event
	// before open, after open, before close, after close
	public delegate void OpenEvent();
	public delegate void CloseEvent(UIPanel uipanel);
	// public Action onVisible;
	public event OpenEvent evtOpen = null;
	public event CloseEvent evtClose = null;

	// block throughclick
	public bool isBlockClick = true;

	// public GameObject btnClose = null;
	protected CanvasGroup canvasGroup = null;

	public Transform parent
	{
		set
		{
			transform.SetParent(value, false);
		}
	}

	public bool visible {
		get { return gameObject.activeSelf; }
	}

	public virtual void Show()
	{
		gameObject.SetActive(true);
		canvasGroup.interactable = true;
	}

	public virtual void Close()
	{
		gameObject.SetActive(false);
		canvasGroup.interactable = false;

		if (evtClose != null)
			evtClose(this);
	}

	// if Android, click back button to trigger close event

	public virtual void OnClickBtnClose()
	{
		Close();
	}
}
