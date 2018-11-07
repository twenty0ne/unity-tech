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
public class UIManager : MonoSingleton<UIManager> 
{
    public enum UIOpenType
    {
        PUSH,
        REPLACE,
    }
        
    private class UIPanelInfo
    {
        public string name;
        public UIPanel panel;
        public float openTime;
        public float closeTime;

        // public UIPanelInfo child; // for menu
        // public UIPanelInfo parent;  // for dialog

        public bool active; 
        public bool forbidClean = false;
    }
       
    public const string PATH_PREFAB_MENU = "Prefabs/UI/Menu/";
    public const string PATH_PREFAB_DIALOG = "Prefabs/UI/Dialog/";
    public const string PATH_PREFAB_WIDGET = "Prefabs/UI/WIdget/";

    public const float INTERVAL_CLEAN_CACHE = 10;
    public const float TIME_MAX_CACHE = 20;

    private UIRoot m_uiRoot = null;
    private UIPanelInfo m_topPanelInfo = null;

    private List<UIPanelInfo> m_panelStack = new List<UIPanelInfo>();
    private Dictionary<string, UIPanelInfo> m_panelCache = new Dictionary<string, UIPanelInfo>();
    // private Dictionary<string, GameObject> m_uiAssets = new Dictionary<string, GameObject>();

    private float cleanCacheTick = 0f;

    private Transform mainCanvas
    {
        get { 
            return uiRoot.mainCanvas.transform; 
        }
    }

    private Transform backCanvas
    {
        get { return uiRoot.backCanvas.transform; }
    }

    private Transform frontCanvas
    {
        get { return uiRoot.frontCanvas.transform; }
    }

    private UIRoot uiRoot
    {
        get { 
            if (m_uiRoot == null)
            {
                GameObject objUIRoot = GameObject.Find("UIROOT");
                if (objUIRoot == null)
                    Debug.LogError("failed to find UIRoot");
                m_uiRoot = objUIRoot.GetComponent<UIRoot>();
            }
            return m_uiRoot;
        }
    }
 
    // TODO:
    // if top dialog?
    //public bool isUIPanelOver
    //{
    //    get { 
    //        bool ret = m_topPanelInfo != null && m_topPanelInfo.panel.isBlockClick; 
    //        if (ret == false)
    //            ret = m_topPanelInfo.child != null && m_topPanelInfo.child.panel.isBlockClick;
    //        return ret;
    //    }
    //}

    public void Update()
    {
        // TODO
        // 定时清理
        cleanCacheTick += Time.deltaTime;
        if (cleanCacheTick >= INTERVAL_CLEAN_CACHE)
        {
            cleanCacheTick -= INTERVAL_CLEAN_CACHE;

            float curTime = Time.realtimeSinceStartup;
            foreach (var kv in m_panelCache)
            {
                UIPanelInfo upInfo = kv.Value;
                if (upInfo == null || upInfo.active ||
                    upInfo.panel.visible)
                    continue;

                if (upInfo.closeTime + TIME_MAX_CACHE < curTime)
                    continue;

                Debug.Log("Destroy cache panel > " + upInfo.panel.name);
                Destroy(upInfo.panel.gameObject);
                m_panelCache.Remove(kv.Key);
                break;
            }
        }
    }

    public UIPanel OpenMenu(string menuName)
    {
        // Check ExistIn Stack
        UIPanelInfo upInfo = FindPanelInStack(menuName);
        if (upInfo != null)
        {
            MoveToStackTop(upInfo);
        }
        else
        {
            // Check In Cache
            m_panelCache.TryGetValue(menuName, out upInfo);
            if (upInfo != null)
            {
                m_panelStack.Add(upInfo);
            }
            else
            {
                // Check In Scene
                // 没有在 Stack 却在场景中这种情况，是因为作为预加载放入场景中
//                GameObject obj = GameObject.Find(menuName);
//                if (obj == null)
//                {
                    // Load from Assets
                string path = PATH_PREFAB_MENU + menuName + ".prefab";
                GameObject obj = AssetManager.LoadGameObject(path);
                Debug.Assert(obj != null, "CHECK");
                obj.transform.SetParent(mainCanvas, false);
//                }

                UIPanel panel = obj.GetComponent<UIPanel>();
                panel.evtClose += OnMenuClose;
                Debug.Assert(panel != null, "CHECK");

                upInfo = new UIPanelInfo();
                upInfo.name = menuName;
                upInfo.panel = panel;
                m_panelStack.Add(upInfo);

                if (!m_panelCache.ContainsKey(menuName))
                    m_panelCache[menuName] = upInfo;
            }
        }

        if (m_topPanelInfo != null)
            m_topPanelInfo.panel.gameObject.SetActive(false);
        m_topPanelInfo = upInfo;
        m_topPanelInfo.panel.gameObject.SetActive(true);
        return upInfo.panel;
    }

    // TODO:
    // Menu 一般是唯一的，但是 Dialog 可能重复使用，比如 DialogConfirm
    // 所以 PanelCache 的处理需要注意
    public UIPanel OpenDialog(string dialogName)
    {
        Debug.Assert(m_topPanelInfo != null, "CHECK");

        if (m_panelCache.ContainsKey(dialogName))
        {
            UIPanelInfo upInfo = m_panelCache[dialogName];

            //if (upInfo.parent != null)
            //{
            //    upInfo.parent.child = null;
            //    upInfo.parent = null;
            //}

            upInfo.panel.transform.SetParent(mainCanvas, false);
            //upInfo.parent = m_topPanelInfo;
            //m_topPanelInfo.child = upInfo;
            upInfo.panel.gameObject.SetActive(true);

            return upInfo.panel;
        }

        // Load from Assets
        string path = PATH_PREFAB_DIALOG + dialogName + ".prefab";
        GameObject obj = AssetManager.LoadGameObject(path);
        Debug.Assert(obj != null, "CHECK");

        UIPanel panel = obj.GetComponent<UIPanel>();
        panel.evtClose += OnDialogClose;
        Debug.Assert(panel != null, "CHECK");

        UIPanelInfo newUpInfo = new UIPanelInfo();
        newUpInfo.name = dialogName;
        newUpInfo.panel = panel;
        m_panelCache[dialogName] = newUpInfo;

        // if one menu had inclued one dialog
        //Debug.Assert(m_topPanelInfo.child == null, "CHECK");

        //m_topPanelInfo.child = newUpInfo;
        newUpInfo.panel.transform.SetParent(mainCanvas, false);
        //newUpInfo.parent = m_topPanelInfo;
           
        return panel;
    }

    private void OnMenuClose(UIPanel panel)
    {
        Debug.Assert(m_panelStack.Count >= 2, "CHECK");
        Debug.Assert(m_topPanelInfo.panel == panel, "CHECK");

        // pop from stack
        UIPanelInfo upInfo = m_panelStack[m_panelStack.Count - 1];
        m_panelStack.RemoveAt(m_panelStack.Count - 1);

        upInfo = m_panelStack[m_panelStack.Count - 1];
        m_topPanelInfo = upInfo;
        m_topPanelInfo.panel.gameObject.SetActive(true);
    }

    private void OnDialogClose(UIPanel panel)
    {
        UIPanelInfo upInfo = FindPanelInCache(panel);
        if (upInfo == null)
        {
            Debug.LogWarning("failed to find uipanel when dialog close");
            return;
        }

        //Debug.Assert(upInfo.parent != null, "CHECK");
        //upInfo.parent.child = null;
        //upInfo.parent = null;
    }

    private UIPanelInfo FindPanelInStack(string name)
    {
        for (int i = 0; i < m_panelStack.Count; ++i)
        {
            UIPanelInfo upInfo = m_panelStack[i];
            if (upInfo.name == name)
                return upInfo;
        }

        return null;
    }

    private UIPanelInfo FindPanelInCache(UIPanel panel)
    {
        foreach (var kv in m_panelCache)
        {
            if (kv.Value.panel == panel)
                return kv.Value;
        }
        return null;
    }

    private void MoveToStackTop(UIPanelInfo upInfo)
    {
        Debug.Assert(upInfo != null, "CHECK");

        m_panelStack.Remove(upInfo);
        m_panelStack.Add(upInfo);
    }

    private void PopUIPanel()
    {
    }
       
    // TODO
    private void OnMemoryWarning()
    {
    }
}
