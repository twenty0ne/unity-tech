using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
public class UICanvas : MonoBehaviour
{
	private void Awake()
	{
		var arf =	GetComponent<AspectRatioFitter>();
		arf.aspectRatio = Screen.width * 1.0f / Screen.height;
	}
}
