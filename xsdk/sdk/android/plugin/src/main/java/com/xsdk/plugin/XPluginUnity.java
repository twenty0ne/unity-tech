package com.xsdk.plugin;

/**
 * Created by twenty0ne on 2018/9/21.
 */

public class XPluginUnity extends XPlugin {
    private static XPluginUnity pluginUnityImpl = new XPluginUnity();

    public static XPluginUnity getInstance()
    {
        return pluginUnityImpl;
    }

    public static String callNative(String jsonParamString)
    {
        XPluginUnity instance = getInstance();
        return instance.executeNative(jsonParamString);
    }

    public void callEngine(String targetObject, String jsonParamString)
    {
        XPlugin.invokeMethod("com.unity3d.player.UnityPlayer", "UnitySendMessage", new Object[]{ targetObject, "CallEngine", jsonParamString});
    }
}
