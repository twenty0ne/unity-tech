using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 放在最上层，并且不应该阻挡输入
public class Console : UIPanel
{
	private class Info
	{
		public float time;
		public string msg;
	}

	private List<Info> infos = new List<Info>();

	private void Awake()
	{
		
	}

	public void OnEndInput(InputField txtInput)
	{
		string msg = txtInput.text;
		txtInput.text = string.Empty;

		if (string.IsNullOrEmpty(msg))
			return;

		Info info = new Info();
		info.time = Time.realtimeSinceStartup;
		info.msg = msg;

		infos.Add(info);
	}
}
