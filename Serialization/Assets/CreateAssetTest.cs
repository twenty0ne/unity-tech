using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KData
{
	public int id;
	public string name;
}

[CreateAssetMenu(fileName = "TestAsset1", menuName = "Test/TestAsset1", order = 1)]
public class TestAsset1 : ScriptableObject
{
	public int id;
	public string name;
	public List<int> intList = new List<int>();
	public List<KData> kDataList;
	public KData[] kDataArray;
}
