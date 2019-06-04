using UnityEngine;

public class MenuStart : UIMenu
{
	private int clickCount = 0;

	// 下到上：通过 事件 传递
	public void OnClickBtnStart()
	{
		clickCount += 1;
		Debug.Log("xx-- clickCount > " + clickCount);
	}

	private void Update()
	{
		
	}
}
