using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// [CreateAssetMenu(fileName = "MyItem", menuName = "Test/MyItem", order = 1)]
public class MyItem 
{
	public int id;
	public string name;
}

// [Serializable]
public class MyObject : MonoBehaviour
{
	[SerializeField]
	public MyItem myItem;

	void Start()
	{
		
	}

	void Update()
	{
			
	}
}
