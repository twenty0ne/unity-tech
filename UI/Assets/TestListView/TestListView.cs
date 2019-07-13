using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestListView : MonoBehaviour
{
	public GameObject itemPrefab;
	public UIListView listView;

	void Start()
	{
		// Debug.Assert(listView != null, "CHECK");
		// for (int i = 0; i < 20; ++i)
		// {
		// 	var obj = GameObject.Instantiate(itemPrefab);
		// 	// obj.transform.SetParent(listView.transform, false);
		// 	TestListItem item = obj.GetComponent<TestListItem>();
		// 	listView.AddChild(item);

		// 	item.onItemClick = OnItemClick;
		// }

		// test long list
		listView.itemCount = 20;

		// GetComponent<ScrollRect>
	}

	private void OnItemClick(TestListItem item)
	{
		Debug.Log("xx-- TestListView > OnItemClick");
	}

	private void OnListBuilder(int index)
	{

	}
}
