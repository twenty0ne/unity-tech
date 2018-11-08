using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlurryTest : MonoBehaviour
{
	private void Start()
	{
		Flurry.SetLogEnabled(true);
		Flurry.SetUserId ("15704");
#if UNITY_ANDROID
		Flurry.Init(FlurryAPIKey.ANDROID);
#elif UNITY_IOS
		Flurry.Init(FlurryAPIKey.IOS);
#endif
	}

	public void OnClickLogEvent(string eventId)
	{
//		Dictionary<string, string> pp = new Dictionary<string, string>();
//		pp["param0"] = "hello";
//		pp["param1"] = "hello2";
//		FlurryIOS.LogEvent("hello", pp);
//		return;

		if (string.Equals (eventId, "hello2")) {
			Dictionary<string, string> parameters = new Dictionary<string, string> ();
			parameters ["param0"] = "hello";
			parameters ["param1"] = "world";
			Flurry.LogEvent (eventId, parameters);
		} else {
			Flurry.LogEvent (eventId);
		}
	}

	public void OnClickLogPayment()
	{
		Flurry.LogPayment("10coin", "6003", 1, 0.99, "USD", string.Empty);
	}
}
