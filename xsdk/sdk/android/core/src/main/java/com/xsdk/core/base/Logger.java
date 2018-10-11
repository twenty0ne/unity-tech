package com.xsdk.core.base;

import android.util.Log;

import com.xsdk.core.Configuration;

public class Logger {
    public static String TAG = "XSdk";

    public static void methodEnter(String tag) {
        methodEnter(tag, "");
    }

    public static void methodEnter(String tag, String params) {
        String methodName = getCallMethodName(2);

        String msg = String.format("\n[METHOD_ENTER] ===  %s ===\n%s\n\n", new Object[] { methodName, params });
        if (Configuration.getUseLog().booleanValue()) {
            Log.i(tag, msg);
        }
        trace(tag, msg);
    }

    public static void methodExit(String tag) {
        methodExit(tag, "");
    }

    public static void methodExit(String tag, String params) {
        String methodName = getCallMethodName(2);
    }

    private static String getCallMethodName(int deep) {
        StackTraceElement[] trace = new Throwable().fillInStackTrace().getStackTrace();
        if (trace.length < deep + 1) {
            return "unknown.unknown()";
        }
        String className = trace[deep].getClassName();
        className = className.substring(className.lastIndexOf('.') + 1);
        String method = trace[deep].getMethodName();
        return className + "." + method + "()";
    }

    private static void trace(String tag, String msg)
    {
        traceLog("T", tag, msg);
    }

    private static void traceLog(String level, String tag, String msg)
    {
        if (Configuration.getUseLog().booleanValue())
        {
            if ((tag != null) && (tag.length() > 0)){
                tag = TAG + "_" + tag;
            }
            else {
                tag = TAG;
            }
            if (msg == null) {
                msg = "(null)";
            }
        }
    }
}
