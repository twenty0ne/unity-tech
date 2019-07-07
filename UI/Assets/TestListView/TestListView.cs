using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListView : MonoBehaviour
{
	public GameObject itemPrefab;
	public UIListView listView;

	void Start()
	{
		Debug.Assert(listView != null, "CHECK");
		for (int i = 0; i < 20; ++i)
		{
			var obj = GameObject.Instantiate(itemPrefab);
			// obj.transform.SetParent(listView.transform, false);
			listView.AddChild(obj.GetComponent<TestListItem>());
		}
	}

	void Update()
	{
			
	}
}
