using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPageView : UIPanel
{
	public UIPageView pageView;
	public GameObject itemPrefab;

	private void Awake() 
	{
		// pageView = GetComponent<UIPageView>();

		// for (int i = 0; i < 10; ++i)
		// {
		// 	GameObject obj = Instantiate(itemPrefab);
		// 	TestPageViewItem item = obj.GetComponent<TestPageViewItem>();

		// 	pageView.AddChild(item);
		// }
	}

	void Start()
	{
		
	}

	public void OnClick()
	{

	}
}
