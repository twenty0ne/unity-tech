using System.Collections.Generic;
using UnityEngine;

public static class FlurryAndroid
{
#if UNITY_ANDROID
	private const string FLURRYAGENT_CLASS_NAME = "com.flurry.android.FlurryAgent";
	private const string UNITYPLAYER_CLASS_NAME = "com.unity3d.player.UnityPlayer";
	private const string UNITYPLAYER_ACTIVITY_NAME = "currentActivity";

	private static AndroidJavaClass s_flurryAgent;

	private static AndroidJavaClass FlurryAgent
	{
		get
		{
			if (s_flurryAgent == null)
				s_flurryAgent = new AndroidJavaClass(FLURRYAGENT_CLASS_NAME);

			return s_flurryAgent;
		}
	}

	public static void Init(string apiKey)
	{
		using (AndroidJavaClass unityPlayer = new AndroidJavaClass(UNITYPLAYER_CLASS_NAME))
		{
			using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>(UNITYPLAYER_ACTIVITY_NAME))
			{
				FlurryAgent.CallStatic("init", activity, apiKey);
			}
		}
	}

	public static void SetLogEnabled(bool isEnabled)
	{
		FlurryAgent.CallStatic("setLogEnabled", isEnabled);
	}

	public static void SetUserId(string userId)
	{
		FlurryAgent.CallStatic("setUserId", userId);
	}

	public static void SetAppVersion(string version)
	{
		FlurryAgent.CallStatic("setVersionName", version);
	}

	public static void LogEvent(string eventName)
	{
		FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", eventName);
	}

	public static void LogEvent(string eventName, Dictionary<string, string> parameters)
	{
		using (var hashMap = DictionaryToJavaHashMap(parameters))
		{
			FlurryAgent.CallStatic<AndroidJavaObject>("logEvent", eventName, hashMap);
		}
	}

	public static void LogPayment(string productName, string productId, int quantity, double price,
		string currency, string transactionId, Dictionary<string, string> parameters)
	{
		using (var hashMap = DictionaryToJavaHashMap(parameters))
		{
			FlurryAgent.CallStatic<AndroidJavaObject>("logPayment", productName, productId, 
				quantity, price, currency, transactionId, hashMap);
		}
	}

	private static AndroidJavaObject DictionaryToJavaHashMap(Dictionary<string, string> dict)
	{
		var javaObject = new AndroidJavaObject("java.util.HashMap");
		var put = AndroidJNIHelper.GetMethodID(javaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

		foreach (KeyValuePair<string, string> entry in dict)
		{
			using (var key = new AndroidJavaObject("java.lang.String", entry.Key))
			{
				using (var value = new AndroidJavaObject("java.lang.String", entry.Value))
				{
					AndroidJNI.CallObjectMethod(javaObject.GetRawObject(), put, AndroidJNIHelper.CreateJNIArgArray(new object[] { key, value }));
				}
			}
		}

		return javaObject;
	}
#endif
}
