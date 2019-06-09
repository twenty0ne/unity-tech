using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVariable<T>
{
	private T val;

	public static UIVariable<T> operator +(UIVariable<T> a, UIVariable<T> b)
	{
		return null;
	}

	public static UIVariable<T> operator +(UIVariable<T> a, T b)
	{
		return null;
	}

}
