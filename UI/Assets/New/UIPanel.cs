using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : UINode 
{
    // 传递进来的参数
    public Dictionary<string, object> param;

    public delegate void OpenEvent();
    public delegate void CloseEvent(UIPanel uipanel);
    // public Action onVisible;
    public event OpenEvent evtOpen = null;
    public event CloseEvent evtClose = null;

    // block throughclick
    public bool isBlockClick = true;

    public virtual void Close()
    {
        gameObject.SetActive(false);

        if (evtClose != null)
            evtClose(this);
    }
}
