using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListView : MonoBehaviour
{
	public GameObject itemPrefab;

	void Start()
	{

		return;
		// UIListView listView = new UIListView();
		// listView.AddChild()

		// for (int i = 0; i < 10; ++i)
		{
			Debug.Log("xx-- inst item 1");
			GameObject obj = GameObject.Instantiate(itemPrefab);
			Debug.Log("xx-- inst item 2");
			TestListItem it =	obj.GetComponent<TestListItem>();

		}

		{
			Debug.Log("xx-- inst item 3");
			GameObject obj2 = GameObject.Instantiate(itemPrefab);
			Debug.Log("xx-- inst item 4");
		}
	}

	void Update()
	{
			
	}
}
