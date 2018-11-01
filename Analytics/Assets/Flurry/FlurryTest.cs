using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlurryTest : MonoBehaviour
{
	private void Start()
	{
		Flurry.SetLogEnable(true);
		Flurry.SetUserId ("15704");
#if UNITY_ANDROID
		Flurry.Init("BVGBDXRY47W2WJ8386S2");
#elif UNITY_IOS
		Flurry.Init("SCXVX34FPZGDXPGQFZX5");
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
			// Flurry.LogEvent (eventId);
			Flurry.LogPayment("100coin", "10000", 1, 2.99, "USD", string.Empty);
		}
	}
}
