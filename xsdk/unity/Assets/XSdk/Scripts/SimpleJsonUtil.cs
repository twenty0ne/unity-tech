using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public static class SimpleJsonUtil
{
	public static bool TryGetValue(this JsonObject jsonObj, string key, ref string value)
	{
		object objVal = null;
		if (jsonObj.TryGetValue(key, out objVal))
		{
			value = System.Convert.ToString(objVal);
			return true;
		}
		return false;
	}

	public static bool TryGetValue(this JsonObject jsonObj, string key, ref int value)
	{
		object objVal = null;
		if (jsonObj.TryGetValue(key, out objVal))
		{
			value = System.Convert.ToInt32(objVal);
			return true;
		}
		return false;
	}
}
