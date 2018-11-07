using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public static class FlurryIOS
{
#if UNITY_IOS
	[DllImport("__Internal")]
	private static extern void StartSessionImpl(string apiKey);
	[DllImport("__Internal")]
	private static extern void SetDebugLogEnabledImpl(bool isEnabled);
	[DllImport("__Internal")]
	private static extern void SetUserIdImpl(string userId);
	[DllImport("__Internal")]
	private static extern void SetAppVersionImpl(string version);
	[DllImport("__Internal")]
	private static extern void LogEventImplA(string eventName);
	[DllImport("__Internal")]
	private static extern void LogEventImplB(string eventName, string parameters);
	[DllImport("__Internal")]
	private static extern void StartPaymentObserverImpl();

	public static void Init(string apiKey)
	{
		StartSessionImpl(apiKey);
		StartPaymentObserverImpl ();
	}

	public static void SetLogEnabled(bool isEnabled)
	{
		SetDebugLogEnabledImpl(isEnabled);
	}

	public static void SetUserId(string userId)
	{
		SetUserIdImpl(userId);
	}

	public static void SetAppVersion(string version)
	{
		SetAppVersionImpl(version);
	}

	public static void LogEvent(string eventName)
	{
		LogEventImplA(eventName);
	}

	public static void LogEvent(string eventName, Dictionary<string, string> parameters)
	{
		string strParameters = DictionaryToString(parameters);
		LogEventImplB(eventName, strParameters);
	}

	private static string DictionaryToString(Dictionary<string, string> dict)
	{
		StringBuilder sb = new StringBuilder();

		bool isFirstKey = true;
		foreach (var kv in dict)
		{
			if (isFirstKey)
				isFirstKey = false;
			else
				sb.Append("|;");

			sb.Append(kv.Key);
			sb.Append("|:");
			sb.Append(kv.Value);
		}

		return sb.ToString();
	}
#endif
}
