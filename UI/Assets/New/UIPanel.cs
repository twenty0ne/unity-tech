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
        gameObject.SetActive(false);

        if (evtClose != null)
            evtClose(this);
    }

    public bool visible
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }

	// if Android, click back button to trigger close event

    public virtual void OnClickBtnClose()
    {
        Close();
    }
}
