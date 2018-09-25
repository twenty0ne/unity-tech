package com.xsdk.core.base;

/**
 * Created by twenty0ne on 2018/9/21.
 */

public class Logger {
    public static String TAG = "XSdk";

    public static void enterMethod(String tag) {
        enterMethod(tag, "");
    }

    public static void enterMethod(String tag, String params) {
        String methodName = getCallMethodName(2);
    }

    public static void exitMethod(String tag) {
        exitMethod(tag, "");
    }

    public static void exitMethod(String tag, String params) {
        String methodName = getCallMethodName(2);
    }

    private static String getCallMethodName(int deep) {
        StackTraceElement[] trace = new Throwable().fillInStackTrace().getStackTrace();
        if (trace.length < deep + 1) {
            return "unknown.unknown()";
        }
        String className = trace[deep].getClassName();
        className = className.substring(className.lastIndexOf('.' + 1));
        String method = trace[deep].getMethodName();
        return className + "." + method + "()";
    }
}
