using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEditor;

public class AssetInfo
{
	public string path;
	public UnityEngine.Object[] objs;
}

public class AssetManager // : MonoSingleton<AssetManager>
{
	// TODO
	// cache LoadAsset
	static Dictionary<string, AssetInfo> assetsCache = new Dictionary<string, AssetInfo>();

	private static T LoadAsset<T>(string path) where T : UnityEngine.Object
	{
		try
		{
#if UNITY_EDITOR
			if (assetsCache.ContainsKey(path))
			{
				AssetInfo assInfo = assetsCache[path];
				if (assInfo.objs != null && assInfo.objs.Length > 0)
					return (T)assetsCache[path].objs[0];
			}

			if (!path.Contains("Assets/"))
				path = "Assets/" + path;

			var ass = (T)AssetDatabase.LoadAssetAtPath<T>(path);
			if (ass == null)
			{
				Debug.LogWarning("faild to load asset at path > " + path);
			}
			else
			{
				// Cache
				AssetInfo newAssInfo = new AssetInfo();
				newAssInfo.path = path;
				newAssInfo.objs = new UnityEngine.Object[1];
				newAssInfo.objs[0] = ass;
				assetsCache[path] = newAssInfo;
			}

			return ass;
#else
			throw new NotImplementedException();
#endif
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			return null;
		}
	}

	private static UnityEngine.Object[] LoadAllAsset(string path)
	{
		try
		{
			#if UNITY_EDITOR
			if (assetsCache.ContainsKey(path))
			{
				AssetInfo assInfo = assetsCache[path];
				if (assInfo.objs != null && assInfo.objs.Length > 0)
					return assetsCache[path].objs;
			}

			if (!path.Contains("Assets/"))
				path = "Assets/" + path;
			UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);

			// Cache
			AssetInfo newAssInfo = new AssetInfo();
			newAssInfo.path = path;
			newAssInfo.objs = objs;
			assetsCache[path] = newAssInfo;

			return objs;
			#else
			throw new NotImplementedException();
			#endif
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			return null;
		}
	}
		
	public static T LoadConfig<T>(string path) where T : class, new()
	{
		try
		{
			#if UNITY_EDITOR 
			var assText = LoadAsset<TextAsset>("Configs/" + path);
			if (assText == null)
			{
				Debug.Log("failed to load config > " + path);
				return null;
			}
			return JsonUtility.FromJson<T>(assText.ToString());
			   
			#else
			throw new NotImplementedException();
			// var assText = Resources.Load<TextAsset>(path);
			// return JsonUtility.FromJson<T>(assText.ToString());
			#endif
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
			return null;
		}
	}

	public static GameObject LoadGameObject(string path)
	{
		try
		{
#if UNITY_EDITOR
			var ass = LoadAsset<GameObject>(path);
			if (ass == null)
			{
				Debug.LogWarning("failed to load gameobject > " + path);
			}
			return UnityEngine.Object.Instantiate(ass);
#else
			throw new NotImplementedException();
#endif
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
			return null;
		}
	}
		
	public static Sprite LoadSprite(string path)
	{
		try
		{
			Sprite spr = null;
			// sprite sheet
			if (path.Contains("@ss"))
			{
				string[] args = path.Split('/');
				if (args.Length < 2)
				{
					Debug.LogError("check args length != 2");
					return null;
				}

				string sprName = args[1].Replace("@ss", "");
				
				#if UNITY_EDITOR
				UnityEngine.Object[] sprs = LoadAllAsset("Sprites/" + args[0]);
				#else
				// UnityEngine.Object[] sprs = Resources.LoadAll("Sprites/" + args[0]);
				throw new NotImplementedException();
				#endif
				for (int i = 0; i < sprs.Length; ++i)
				{
					if (sprs[i] is Sprite)
					{
						Sprite obj = (Sprite)sprs[i];
						if (obj.name == sprName)
						{   
							spr = obj;
							break;
						}
					}
				}
			}
			else
			{
				#if UNITY_EDITOR
				spr = LoadAsset<Sprite>("Sprites/" + path);
				#else
				spr = Resources.Load<Sprite>("Sprites/" + path);
				#endif
			}

			if (spr == null)
				Debug.LogWarning("failed to load sprite > " + path);
			return spr;   
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
			return null;
		}
	}
}
