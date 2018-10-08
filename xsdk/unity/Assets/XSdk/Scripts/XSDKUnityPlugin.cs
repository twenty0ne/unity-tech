using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

namespace XSDK
{
	public class XSDKUnityPlugin : MonoBehaviour
	{
		private static XSDKUnityPlugin _instance = null;

#if UNITY_ANDROID
		private static AndroidJavaClass _androidClass;
#endif

		public static string targetObject;
		public static int handlerId = 0;
		public static Dictionary<int, object> callbackHandler = new Dictionary<int, object>();

		public static XSDKUnityPlugin Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObject("XSDKUnityPlugin").AddComponent<XSDKUnityPlugin>();
				}
				return _instance;
			}
		}

		void Awake()
		{
			if (_instance && _instance.GetInstanceID() != GetInstanceID())
			{
				DestroyImmediate(gameObject);
			}
			else
			{
				_instance = this;
				DontDestroyOnLoad(gameObject);
			}

			_androidClass = new AndroidJavaClass("com.xsdk.plugin.XPluginUnity");
			targetObject = gameObject.name;
		}

		public JsonObject CallNative(JsonObject jsonParam)
		{
#if UNITY_ANDROID
			string jsonParamString = jsonParam.ToString();
			string resJsonString = _androidClass.CallStatic<string>("callNative", jsonParamString);
			Debug.Log("xx-- CallNative > " + resJsonString);
			return SimpleJson.SimpleJson.DeserializeObject<JsonObject>(resJsonString);
#else
			return new JsonObject();
#endif
		}

		public void CallEngine(string jsonParam)
		{
			Debug.Log("callEngine:" + jsonParam);

			JsonObject resJsonObject = SimpleJson.SimpleJson.DeserializeObject<JsonObject>(jsonParam);

			string className = null;
			resJsonObject.TryGetValue("class", ref className);

			if ("Auth".Equals(className))
			{
				Auth.ExecuteEngine(resJsonObject);
			}
		}

		public static JsonObject CreateParam(string className, string methodName, object handler)
		{
			JsonObject jsonParam = new JsonObject();
			jsonParam.Add("targetObject", targetObject);
			jsonParam.Add("class", className);
			jsonParam.Add("method", methodName);

			if (handler != null)
				jsonParam.Add("handler", PushHandler(handler));
			return jsonParam;
		}

		public static int PushHandler(object handler)
		{
			int newHandlerId = handlerId++;
			callbackHandler[newHandlerId] = handler;
			return newHandlerId;
		}

		public static object PopHandler(int handlerIdParam)
		{
			object handler = null;
			if (callbackHandler.ContainsKey(handlerIdParam))
			{
				handler = callbackHandler[handlerIdParam];
				callbackHandler.Remove(handlerIdParam);
			}
			return handler;
		}
	}
}


