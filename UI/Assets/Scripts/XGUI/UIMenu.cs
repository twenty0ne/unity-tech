using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 不单独区分 UIMenu, UIDialog
// 只用 FullScreen 区分
public class UIMenu : UIWidget
{
	public bool FullScreen { get; set; }

	// public static T Show<T>()
	// {
	// 	GameObject mu = UIManager.Instance.TryGetMenu(typeof(T));
	// 	Debug.Assert(mu != null && mu.GetComponent<T>() != null, "CHECK");
	// 	return mu.GetComponent<T>();
	// }

	public System.Action onShow;
	public System.Action onClose;

	public void Show()
	{

	}

	public void Hide()
	{

	}

	public virtual void OnShow()
	{

	}

	public virtual void OnHide()
	{

	}

}
