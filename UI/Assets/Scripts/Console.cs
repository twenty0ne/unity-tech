using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 放在最上层，并且不应该阻挡输入
public class Console : UIPanel
{
	private class Info
	{
		public int time;
		public string msg;
	}

	private List<Info> infos = new List<Info>();

	public void OnEndInput(string msg)
	{

	}
}
