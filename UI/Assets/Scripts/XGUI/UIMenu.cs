using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 不单独区分 UIMenu, UIDialog
// 只用 FullScreen 区分
public class UIMenu : UIWidget
{
	public bool fullScreen;

	// public static T Show<T>()
	// {
	// 	GameObject mu = UIManager.Instance.TryGetMenu(typeof(T));
	// 	Debug.Assert(mu != null && mu.GetComponent<T>() != null, "CHECK");
	// 	return mu.GetComponent<T>();
	// }

	public System.Action onShow;
	public System.Action onClose;

	protected bool _bActive = true;
	protected Vector3 _originPos;

	private void Start()
	{
		_originPos = _rt.anchoredPosition;
	}

	public void Show()
	{
		Debug.Log("Menu " + name + " Show.");
		OnShow();
	}

	public void Hide()
	{
		Debug.Log("Menu " + name + " Hide.");
		OnHide();
	}

	public void Active()
	{
		Debug.Log("Menu " + name + " Active.");

		if (_bActive)
			return;

		_bActive = true;
		_rt.anchoredPosition = _originPos;

		OnActive();
	}

	public void Deactive()
	{
		Debug.Log("Menu " + name + " Deactive.");

		if (!_bActive)
			return;

		_bActive = false;
		_rt.anchoredPosition = _originPos + new Vector3(0, Screen.height * 2, 0);

		OnDeactive();
	}

	protected virtual void OnShow()
	{
		Debug.Log("Menu " + name + " OnShow.");
	}

	protected virtual void OnHide()
	{
		Debug.Log("Menu " + name + " OnHide.");
	}

	protected virtual void OnActive()
	{
		Debug.Log("Menu " + name + " OnActive.");
	}

	protected virtual void OnDeactive()
	{
		Debug.Log("Menu " + name + " OnDeactive.");
	}

	public void OnClickBtnClose()
	{
		Debug.Log("Menu " + name + " OnClickBtnClose.");
		UIManager.Instance.CloseMenu(this);
	}
}
