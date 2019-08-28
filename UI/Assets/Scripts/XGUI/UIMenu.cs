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
		_originPos = _rt.position;
	}

	public void Show()
	{
	}

	public void Hide()
	{

	}

	public void Active()
	{
		if (_bActive)
			return;

		_bActive = true;
		_rt.position = _originPos;
	}

	public void Deactive()
	{
		if (!_bActive)
			return;

		_bActive = false;
		_rt.position = _originPos + new Vector3(0, 19200, 0);
	}

	public virtual void OnShow()
	{

	}

	public virtual void OnHide()
	{

	}

	public virtual void OnActive()
	{

	}

	public virtual void OnDeactive()
	{

	}
}
