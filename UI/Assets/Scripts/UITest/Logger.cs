using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
	public static void Debug(object msg)
	{
		UnityEngine.Debug.Log(msg);
	}

	public static void Debug(string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(format, args);
	}

	public static void Info(object msg)
	{
		UnityEngine.Debug.Log(msg);
	}

	public static void Info(string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(format, args);
	}

	public static void Warning(object msg)
	{
		UnityEngine.Debug.LogWarning(msg);
	}

	public static void Warning(string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(format, args);
	}

	public static void Error(object msg)
	{
		UnityEngine.Debug.LogError(msg);
	}

	public static void Error(string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(format, args);
	}
}
