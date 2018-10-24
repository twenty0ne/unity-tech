using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

namespace XSDK
{
	public class Auth
	{
		public delegate void onAuthInit(ResultAPI result, AuthInitResult authInitResult);

		public static void Init(onAuthInit listener)
		{
			JsonObject jsonParam = XSDKUnityPlugin.CreateParam("Auth", "init", listener);
			XSDKUnityPlugin.Instance.CallNative(jsonParam);
		}

		public static void ExecuteEngine(JsonObject resJsonObject)
		{
			string methodName = null;
			resJsonObject.TryGetValue("method", ref methodName);

			int handlerId = -1;
			resJsonObject.TryGetValue("handler", ref handlerId);

			// handleId 指向上层游戏逻辑回调
			object handler = (object)XSDKUnityPlugin.PopHandler(handlerId);
			if (handler == null)
				Debug.LogWarning("Auth.ExecuteEngine cant find handler for method > " + methodName);

			if ("init".Equals(methodName))
			{
				if (handler == null)
					return;

				onAuthInit listener = handler as onAuthInit;
				if (listener != null)
					listener(new ResultAPI(resJsonObject["resultAPI"] as JsonObject), new AuthInitResult(resJsonObject["authInitResult"] as JsonObject));
			}
			else if ("login".Equals(methodName))
			{
				if (handler == null)
					return;

			}
			else if ("logout".Equals(methodName))
			{

			}
		}
	}

	public class AuthInitResult
	{
		public AuthInitResult(JsonObject resJsonParam)
		{
			
		}
	}
}
