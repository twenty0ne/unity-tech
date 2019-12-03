using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(CanvasGroup))]
// UIPanel 是一组 UIWidget 集合，便于控制局部功能
public class UIPanel : UIWidget 
{
	// 传递进来的参数
	// public Dictionary<string, object> param;

	// TODO:
	// more event
	// before open, after open, before close, after close
	// Open 与 Show 区别
	// Open 是第一次打开
	// Show 是每次显示出来
	// public delegate void OpenEvent();
	// public delegate void ShowEvent();
	// public delegate void HideEvent();
	// public delegate void CloseEvent(UIPanel up);
	// public Action onVisible;
	// public event OpenEvent onOpen = null;
	// public event ShowEvent onShow = null;
	// public event HideEvent onHide = null;
	// public event CloseEvent onClose = null;

	// block throughclick
	// public bool isBlockClick = true;

	// public GameObject btnClose = null;
	// protected CanvasGroup canvasGroup = null;

	// protected List<UIWidget> widgets = new List<UIWidget>();

	// public Transform parent
	// {
	// 	set
	// 	{
	// 		transform.SetParent(value, false);
	// 	}
	// }

	// public bool visible 
	// {
	// 	get { return gameObject.activeSelf; }
	// }

	// public virtual void Open()
	// {
	// 	if (onOpen != null)
	// 		onOpen();
	// }

	// public virtual void Show(/*bool isAutoByMgr */)
	// {
	// 	gameObject.SetActive(true);
	// 	canvasGroup.interactable = true;

	// 	if (onShow != null)
	// 		onShow();
	// }

	// public virtual void Hide(/* bool isAutoByMgr */)
	// {
	// 	gameObject.SetActive(false);

	// 	if (onHide != null)
	// 		onHide();
	// }

	// public virtual void Close()
	// {
	// 	gameObject.SetActive(false);
	// 	canvasGroup.interactable = false;

	// 	if (onClose != null)
	// 		onClose(this);
	// }

	// if Android, click back button to trigger close event

	// public virtual void OnClickBtnClose()
	// {
	// 	Close();
	// }

	// public virtual void AddChild(UIWidget uw)
	// {
	// }
}
