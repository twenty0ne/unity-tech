using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
// 界面出现的方式可以参考 ojbect-c storyboard
// push, model, popover, replace, custom
// 界面之间的通信可以通过传递参数/或者使用消息

// TODO:
// 如果是预先拖入 Scene 中的 UIPanel，说明是要在场景加载的时候初始化的
// 这部分如何放入 uipanelStack, 如何避免重复打开
// TODO:
// 可以将预加载的 UI 放在 Scene 统一一个名字下面，比如 ui_preload
// layer_main 按照 menu, dialog 规则
// 其他 layer 随意添加
// NOTE:
// 行为定义
// menu 一次只有一个
// dialog 应该是保持一次只有一个，点击之后就应该关闭然后执行相关操作
// public class UIManager : MonoSingleton<UIManager> 
// {
// 	public enum UIOpenType
// 	{
// 		PUSH,
// 		REPLACE,
// 	}
		
// 	private class UIPanelInfo
// 	{
// 		public string name;
// 		public UIPanel panel;
// 		public float openTime;
// 		public float closeTime;

// 		// public UIPanelInfo child; // for menu
// 		// public UIPanelInfo parent;  // for dialog

// 		// public bool active; 
// 		public bool forbidClean = false;
// 	}
	   
// 	public const string PATH_PREFAB_MENU = "Prefabs/UI/Menu/";
// 	public const string PATH_PREFAB_DIALOG = "Prefabs/UI/Dialog/";
// 	public const string PATH_PREFAB_WIDGET = "Prefabs/UI/WIdget/";
// 	public const string PATH_PREFAB_PANEL = "Prefabs/UI/Panel/";

// 	public const float INTERVAL_CLEAN_CACHE = 10;
// 	public const float TIME_MAX_CACHE = 20;

// 	// private UIRoot m_uiRoot = null;
// 	private UIPanelInfo m_topPanelInfo = null;

// 	// TODO:
// 	// use LRU
// 	private List<UIPanelInfo> m_panelStack = new List<UIPanelInfo>();
// 	// private Dictionary<string, UIPanelInfo> m_panelCache = new Dictionary<string, UIPanelInfo>();
// 	// because dialog 可能重名，所以用 list 替换  dictionary
// 	private List<UIPanelInfo> m_panelCache = new List<UIPanelInfo>();
// 	// private Dictionary<string, GameObject> m_uiAssets = new Dictionary<string, GameObject>();

// 	private float cleanCacheTick = 0f;

// 	// public Transform mainCanvas
// 	// {
// 	// 	get { 
// 	// 		return uiRoot.mainCanvas.transform; 
// 	// 	}
// 	// }

// 	// public Transform backCanvas
// 	// {
// 	// 	get { return uiRoot.backCanvas.transform; }
// 	// }

// 	// public Transform frontCanvas
// 	// {
// 	// 	get { return uiRoot.frontCanvas.transform; }
// 	// }

// 	// private UIRoot uiRoot
// 	// {
// 	// 	get { 
// 	// 		if (m_uiRoot == null)
// 	// 		{
// 	// 			GameObject objUIRoot = GameObject.Find("UIROOT");
// 	// 			if (objUIRoot == null)
// 	// 				Debug.LogError("failed to find UIRoot");
// 	// 			m_uiRoot = objUIRoot.GetComponent<UIRoot>();
// 	// 		}
// 	// 		return m_uiRoot;
// 	// 	}
// 	// }
 
// 	// TODO:
// 	// if top dialog?
// 	//public bool isUIPanelOver
// 	//{
// 	//    get { 
// 	//        bool ret = m_topPanelInfo != null && m_topPanelInfo.panel.isBlockClick; 
// 	//        if (ret == false)
// 	//            ret = m_topPanelInfo.child != null && m_topPanelInfo.child.panel.isBlockClick;
// 	//        return ret;
// 	//    }
// 	//}

// 	public void Update()
// 	{
// 		return;

// 		// TODO
// 		// 定时清理
// 		cleanCacheTick += Time.deltaTime;
// 		if (cleanCacheTick >= INTERVAL_CLEAN_CACHE)
// 		{
// 			cleanCacheTick -= INTERVAL_CLEAN_CACHE;

// 			float curTime = Time.realtimeSinceStartup;
// 			foreach (var upInfo in m_panelCache)
// 			{
// 				if (upInfo == null || upInfo.panel.visible)
// 					continue;

// 				if (upInfo.closeTime + TIME_MAX_CACHE < curTime)
// 					continue;

// 				Debug.Log("Destroy cache panel > " + upInfo.panel.name);
// 				Destroy(upInfo.panel.gameObject);
// 				m_panelCache.Remove(upInfo);
// 				break;
// 			}
// 		}
// 	}

// 	public UIPanel OpenMenu(string menuName)
// 	{
// 		// Check ExistIn Stack
// 		UIPanelInfo upInfo = FindPanelInStack(menuName);
// 		if (upInfo != null)
// 		{
// 			MoveToStackTop(upInfo);
// 		}
// 		else
// 		{
// 			// Check In Cache
// 			upInfo = FindPanelInStack(menuName);
// 			if (upInfo != null)
// 			{
// 				m_panelStack.Add(upInfo);
// 			}
// 			else
// 			{
// 				// Check In Scene
// 				// 没有在 Stack 却在场景中这种情况，是因为作为预加载放入场景中
// //                GameObject obj = GameObject.Find(menuName);
// //                if (obj == null)
// //                {
// 					// Load from Assets
// 				string path = PATH_PREFAB_MENU + menuName + ".prefab";
// 				GameObject obj = AssetManager.LoadGameObject(path);
// 				Debug.Assert(obj != null, "CHECK");
// 				obj.transform.SetParent(mainCanvas, false);
// //                }

// 				UIPanel panel = obj.GetComponent<UIPanel>();
// 				panel.onClose += OnMenuClose;
// 				Debug.Assert(panel != null, "CHECK");
// 				panel.Open();

// 				upInfo = new UIPanelInfo();
// 				upInfo.name = menuName;
// 				upInfo.panel = panel;

// 				m_panelStack.Add(upInfo);
// 				m_panelCache.Add(upInfo);
// 			}
// 		}

// 		if (m_topPanelInfo != null)
// 			m_topPanelInfo.panel.Hide();
// 		m_topPanelInfo = upInfo;
// 		m_topPanelInfo.panel.Show();
// 		return upInfo.panel;
// 	}

// 	// TODO:
// 	// Menu 一般是唯一的，但是 Dialog 可能重复使用，比如 DialogConfirm
// 	// 所以 PanelCache 的处理需要注意
// 	public UIPanel OpenDialog(string dialogName)
// 	{
// 		Debug.Assert(m_topPanelInfo != null, "CHECK");

// 		foreach (var upInfo in m_panelCache)
// 		{
// 			if (upInfo.name != dialogName)
// 				continue;

// 			if (upInfo.panel != null && !upInfo.panel.visible)
// 			{
// 				upInfo.panel.parent = mainCanvas;
// 				upInfo.panel.Show();

// 				return upInfo.panel;
// 			}
// 		}

// 		// Load from Assets
// 		string path = PATH_PREFAB_DIALOG + dialogName + ".prefab";
// 		GameObject obj = AssetManager.LoadGameObject(path);
// 		Debug.Assert(obj != null, "CHECK");

// 		UIPanel panel = obj.GetComponent<UIPanel>();
// 		panel.onClose += OnDialogClose;
// 		Debug.Assert(panel != null, "CHECK");

// 		UIPanelInfo newUpInfo = new UIPanelInfo();
// 		newUpInfo.name = dialogName;
// 		newUpInfo.panel = panel;
// 		m_panelCache.Add(newUpInfo);

// 		// if one menu had inclued one dialog
// 		//Debug.Assert(m_topPanelInfo.child == null, "CHECK");

// 		//m_topPanelInfo.child = newUpInfo;
// 		panel.parent = mainCanvas;
// 		panel.Show();
// 		//newUpInfo.parent = m_topPanelInfo;
		   
// 		return panel;
// 	}

// 	// 比 OpenMenu, OpenDialog 更自由的打开方式
// 	// 谨慎使用
// 	public UIPanel OpenPanel(string panelName, Transform parent)
// 	{
		

// 		return null;
// 	}

// 	private void OnMenuClose(UIPanel panel)
// 	{
// 		Debug.Assert(m_panelStack.Count >= 2, "CHECK");
// 		Debug.Assert(m_topPanelInfo.panel == panel, "CHECK");

// 		// pop from stack
// 		UIPanelInfo upInfo = m_panelStack[m_panelStack.Count - 1];
// 		m_panelStack.RemoveAt(m_panelStack.Count - 1);

// 		upInfo = m_panelStack[m_panelStack.Count - 1];
// 		m_topPanelInfo = upInfo;
// 		m_topPanelInfo.panel.gameObject.SetActive(true);
// 	}

// 	private void OnDialogClose(UIPanel panel)
// 	{
// 		UIPanelInfo upInfo = FindPanelInCache(panel);
// 		if (upInfo == null)
// 		{
// 			Debug.LogWarning("failed to find uipanel when dialog close");
// 			return;
// 		}

// 		//Debug.Assert(upInfo.parent != null, "CHECK");
// 		//upInfo.parent.child = null;
// 		//upInfo.parent = null;
// 	}

// 	private UIPanelInfo FindPanelInStack(string name)
// 	{
// 		for (int i = 0; i < m_panelStack.Count; ++i)
// 		{
// 			UIPanelInfo upInfo = m_panelStack[i];
// 			if (upInfo.name == name)
// 				return upInfo;
// 		}

// 		return null;
// 	}

// 	private UIPanelInfo FindPanelInCache(UIPanel panel)
// 	{
// 		foreach (var upInfo in m_panelCache)
// 		{
// 			if (upInfo.panel == panel)
// 				return upInfo;
// 		}
// 		return null;
// 	}

// 	private void MoveToStackTop(UIPanelInfo upInfo)
// 	{
// 		Debug.Assert(upInfo != null, "CHECK");

// 		m_panelStack.Remove(upInfo);
// 		m_panelStack.Add(upInfo);
// 	}

// //    private void PopUIPanel()
// //    {
// //    }
	   
// 	// TODO
// 	private void OnMemoryWarning()
// 	{
// 	}
// }

using System;

public class MenuStackInfo
{
	public string name;
	public UIMenu menu;
}

public class MenuCacheInfo
{
	public UIMenu menu;
}

// menu cache: all opened menu, 如何处理曾经打开过的多个窗口，比如确认框
// menu stack: record menu open 
public class UIManager : MonoSingleton<UIManager>
{
	public const string PATH_PREFAB_MENU = "Prefabs/";

	private Dictionary<Type, GameObject> menuCaches = new Dictionary<Type, GameObject>();
	private Transform _mainCanvas = null;

	// private List<MenuStackInfo> _menuStack = new List<MenuStackInfo>();
	// private UIMenu _currMenu = null;
	// private List<ui>
	private List<UIMenu> _menuStack = new List<UIMenu>();
	// private List<UIMenu> _showMenus = new List<UIMenu>();
	private List<UIMenu> _menuCache = new List<UIMenu>();
	private UIMenu _topMenu = null;

	protected override void Awake() 
	{
		base.Awake();

		_mainCanvas = GameObject.Find("MainCanvas").transform;
		Debug.Assert(_mainCanvas != null, "CHECK");
	}

	private void Update()
	{
		float dt = Time.deltaTime;
		for (int i = 0; i < _menuStack.Count; ++i)
		{
			_menuStack[i].Tick(dt);
		}
	}

	public GameObject TryGetMenu(Type tp)
	{
		if (menuCaches.ContainsKey(tp))
			return menuCaches[tp];

		// Load from Assets
		string path = PATH_PREFAB_MENU + tp.ToString() + ".prefab";
		GameObject obj = AssetManager.LoadGameObject(path);
		Debug.Assert(obj != null, "CHECK");
		obj.transform.SetParent(_mainCanvas.transform, false);

		return obj;
	}

	public UIMenu OpenMenu(string name)
	{
		// UIMenu newMenu = null;
		// MenuStackInfo minfo = FindMenuStackInfo(name);
		// if (minfo != null)
		// {
		// 	// MoveToStackTop(upInfo);
		// 	newMenu = minfo.menu;
		// 	_menuStack.Remove(minfo);
		// }
		UIMenu menu = FindMenuInCache(name);
		if (menu == null)
		{
			string path = PATH_PREFAB_MENU + name + ".prefab";
			GameObject obj = AssetManager.LoadGameObject(path);
			Debug.Assert(obj != null, "CHECK");
			obj.transform.SetParent(_mainCanvas, false);

			menu = obj.GetComponent<UIMenu>();
			Debug.Assert(menu, "CHECK");
			menu.onHide += OnMenuHide;
			_menuCache.Add(menu);
		}

		// set top menu
		if (_topMenu)
		{
			_topMenu.Deactive(menu);
		}
		_topMenu = menu;

		// 将 menu 放在栈顶
		_menuStack.Remove(menu);
		_menuStack.Add(menu);

		menu.Show();
		return menu;
	}

	public void CloseMenu(UIMenu menu)
	{
		menu.Deactive();
		menu.Hide();

		// 关闭的界面从 stack 中清除
		_menuStack.Remove(menu);
		// 从堆栈中返回最后一个
		Debug.Assert(_menuStack.Count > 0, "CHECK");
		UIMenu stackTopMenu = _menuStack[_menuStack.Count - 1];
		stackTopMenu.Show();
		_topMenu = stackTopMenu;
	}

	private UIMenu FindMenuInStack(string name)
	{
		UIMenu menu = _menuStack.Find(x => x.name == name);
		return menu;
	}

	private UIMenu FindMenuInCache(string name)
	{
		UIMenu menu = _menuCache.Find(x => x.name == name);
		return menu;
	}

	// private void MoveToMenuStackTop(UIPanelInfo upInfo)
	// {
	// 	Debug.Assert(upInfo != null, "CHECK");

	// 	m_panelStack.Remove(upInfo);
	// 	m_panelStack.Add(upInfo);
	// }

	private void OnMenuHide()
	{

	}

	public void OnReceiveMemory()
	{
		
	}
}