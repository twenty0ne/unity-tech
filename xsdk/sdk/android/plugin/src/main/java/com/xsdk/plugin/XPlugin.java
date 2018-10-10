package com.xsdk.plugin;

import com.xsdk.core.ResultAPI;
import com.xsdk.core.base.Logger;

import org.json.JSONObject;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Objects;

public abstract class XPlugin implements ICallEngine{

    private Auth auth;


    public XPlugin() {
        this.auth = new Auth(this);
    }

    public String executeNative(String jsonParamString) {
        try
        {
            JSONObject jsonParam = new JSONObject(jsonParamString);
            String className = jsonParam.optString("class");
            String methodName = jsonParam.optString("method");
            String targetObject = jsonParam.optString("targetObject");
            if ("Auth".equals(className)) {
                return (String)invokeMethod(this.auth, methodName, new Object[] { targetObject, jsonParam });
            }
        }
        catch (Exception localException) {}
        return "";
    }

    public static Object invokeMethod(String className, String methodName, Object[] objList)
    {
        try
        {
            Class<?> cls = Class.forName(className);
            Method[] methods = cls.getMethods();
            for (int i = 0; i < methods.length; i++) {
                if (methods[i].getName().equals(methodName))
                {
                    if (methods[i].getReturnType().getName().equals("void"))
                    {
                        methods[i].invoke(null, objList);
                        return null;
                    }
                    return methods[i].invoke(null, objList);
                }
            }
        }
        catch (IllegalAccessException localIllegalAccessException) {}
        catch (InvocationTargetException localInvocationTargetException) {}
        catch (Exception localException) {}
        return "";
    }

    public static Object invokeMethod(Object obj, String methodName, Object[] objList)
    {
        try
        {
            Method[] methods = obj.getClass().getMethods();
            for (int i = 0; i < methods.length; i++) {
                if (methods[i].getName().equals(methodName))
                {
                    if (methods[i].getReturnType().getName().equals("void"))
                    {
                        methods[i].invoke(obj, objList);
                        return null;
                    }
                    return methods[i].invoke(obj, objList);
                }
            }
        }
        catch (IllegalAccessException localIllegalAccessException) {}
        catch (InvocationTargetException localInvocationTargetException) {}
        catch (Exception localException) {}
        return "";
    }

    public static JSONObject createResponse(ResultAPI result, JSONObject jsonParam)
    {
        JSONObject resJsonParam = new JSONObject();
        try
        {
            resJsonParam.put("class", jsonParam.optString("class"));
            resJsonParam.put("method", jsonParam.optString("method"));
            resJsonParam.put("handler", jsonParam.optString("handler"));
            if (result != null) {
                resJsonParam.put("resultAPI", result.toJson());
            }
        }
        catch (Exception localException) {}
        return resJsonParam;
    }
}
