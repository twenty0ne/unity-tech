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
	public System.Action onHide;

	protected bool _bShow = true;
	protected bool _bActive = true;
	protected bool _bOutOfScreen = false;
	protected Vector3 _originPos;

	private void Start()
	{
		_originPos = _rt.anchoredPosition;
	}

	public void Show()
	{
		Debug.Log("Menu " + name + " Show.");
		// OnBeforeShow();
		// 播放 show 的动画
		OnShow();
	
		_bShow = true;
		_rt.anchoredPosition = _originPos;
	}

	public void Hide()
	{
		Debug.Log("Menu " + name + " Hide.");
		OnHide();

		_bShow = false;
		_rt.anchoredPosition = _originPos + new Vector3(0, Screen.height * 2, 0);
	}

	public override void Active()
	{
		Debug.Log("Menu " + name + " Active.");

		if (_bActive)
			return;

		_bActive = true;
		if (_bOutOfScreen)
		{
			_bOutOfScreen = false;
			_rt.anchoredPosition = _originPos;
		}

		// 
		base.Active();

		OnActive();
	}

	public  void Deactive(UIMenu newTopMenu = null)
	{
		Debug.Log("Menu " + name + " Deactive.");

		if (!_bActive)
			return;

		_bActive = false;
		if (newTopMenu && newTopMenu.fullScreen)
		{
			_bOutOfScreen = true;
			_rt.anchoredPosition = _originPos + new Vector3(0, Screen.height * 2, 0);
		}

		//
		base.Deactive();

		OnDeactive();
	}

	protected virtual void OnShow()
	{
		Debug.Log("Menu " + name + " OnShow.");
		Active();
	}

	protected virtual void OnHide()
	{
		Debug.Log("Menu " + name + " OnHide.");
		Deactive();
	}

	protected override void OnActive()
	{
		Debug.Log("Menu " + name + " OnActive.");
	}

	protected override void OnDeactive()
	{
		Debug.Log("Menu " + name + " OnDeactive.");
	}

	// public void OnClickBtnClose()
	// {
	// 	Debug.Log("Menu " + name + " OnClickBtnClose.");
	// 	UIManager.Instance.CloseMenu(this);
	// }
}
