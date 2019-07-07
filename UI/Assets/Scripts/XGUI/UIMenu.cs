using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
	public static T Show<T>()
	{
		GameObject mu = UIManager.Instance.TryGetMenu(typeof(T));
		Debug.Assert(mu != null && mu.GetComponent<T>() != null, "CHECK");
		return mu.GetComponent<T>();
	}
}
