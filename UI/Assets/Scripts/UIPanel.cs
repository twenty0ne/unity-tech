using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public Transform parent
	{
		set
		{
			transform.SetParent(value, false);
		}
	}

	public bool active
	{
		get { return gameObject.activeSelf; }
		set { gameObject.SetActive(value); }
	}

	protected virtual void Start()
	{
//		Debug.Log(this.GetType().Name + ".Start");
//
//		if (btnClose != null)
//		{
//			// registe close event
//
//		}
	}

    public virtual void Close()
    {
		active = false;

        if (evtClose != null)
            evtClose(this);
    }

	// if Android, click back button to trigger close event

    public virtual void OnClickBtnClose()
    {
        Close();
    }
}
