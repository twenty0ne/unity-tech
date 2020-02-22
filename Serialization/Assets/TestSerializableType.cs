using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{

}

[Serializable]
public class PlayerData : ISerializationCallbackReceiver
{
	public int id;
	public string name;
	public bool isMale;
	public List<int> intList = new List<int>();
	public List<ItemData> itemList = new List<ItemData>();

	public void OnAfterDeserialize()
	{
		Debug.Log("xx-- PlayerData.OnAfterDeserialize");
	}

	public void OnBeforeSerialize()
	{
		Debug.Log("xx-- PlayerData.OnBeforeSerialize");
	}
}

public class TestSerializableType : MonoBehaviour
{
	public void OnClickSerialize()
	{
		Debug.Log("xx-- TestSerializableType.OnClickSerialize");
	}

	public void OnClickDeserialize()
	{
		Debug.Log("xx-- TestSerializableType.OnClickDeserialize");
	}
}
