using System.Collections.Generic;

public class Flurry
{
	public static void Init(string apiKey)
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		FlurryAndroid.Init(apiKey);
#elif UNITY_IOS
		FlurryIOS.Init(apiKey);
#endif
	}

	public static void SetLogEnabled(bool isEnabled)
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		FlurryAndroid.SetLogEnabled(isEnabled);
#elif UNITY_IOS
		FlurryIOS.SetLogEnabled(isEnabled);
#endif
	}

	public static void SetUserId(string userId)
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		FlurryAndroid.SetUserId(userId);
#elif UNITY_IOS
		FlurryIOS.SetUserId(userId);
#endif
	}

	public static void SetAppVersion(string version)
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		FlurryAndroid.SetAppVersion(version);
#elif UNITY_IOS
		FlurryIOS.SetAppVersion(version);
#endif
	}

	public static void LogEvent(string eventName)
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		FlurryAndroid.LogEvent(eventName);
#elif UNITY_IOS
		FlurryIOS.LogEvent(eventName);
#endif
	}

	public static void LogEvent(string eventName, Dictionary<string, string> parameters)
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		FlurryAndroid.LogEvent(eventName, parameters);
#elif UNITY_IOS
		FlurryIOS.LogEvent(eventName, parameters);
#endif
	}

	// NOTE:
	// only for Android
	// @param productName The name of the product purchased.
	// @param productId The id of the product purchased.
	// @param quantity The number of products purchased.
	// @param price The price of the the products purchased in the given currency.
	// @param currency The currency for the price argument.
	// @param transactionId A unique identifier for the transaction used to make the purchase.
	// @param parameters A {@code Map<String, String>} of the parameters which should be submitted with this event.
	// @example: logPayment("candy", "yummy_candy", 1, 2.99, "USD", "123456789", params)
	public static void LogPayment(string productName, string productId, int quantity, double price,
		string currency, string transactionId = null, Dictionary<string, string> parameters = null)
	{
#if UNITY_EDITOR
#elif UNITY_ANDROID
		if (string.IsNullOrEmpty(transactionId))
			transactionId = System.DateTime.Now.ToString("yyyyMMddTHHmmss");
		if (parameters == null)
			parameters = new Dictionary<string, string>();

		FlurryAndroid.LogPayment(productName, productId, quantity, price, currency, transactionId, parameters);
#elif UNITY_IOS
#endif
	}
}
