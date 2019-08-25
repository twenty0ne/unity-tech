using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTest : MonoBehaviour 
{
	MenuMain menuMain = null;

	private void Awake()
	{
		// MenuStart mu = UIMenu.Show<MenuStart>();
	}

	void Start () 
	{
		// UIManager.Instance.OpenMenu("MenuLogin");
	}

	void Update () 
  {
		// if (Input.GetKeyDown(KeyCode.Space))
		// {
		// 	UIManager.Instance.OpenPanel("Console", UIManager.Instance.frontCanvas);
		// }

		// if (Input.GetKeyDown(KeyCode.Space))
		// {
		// 	if (menuMain == null)
		// 	{
		// 		menuMain = UIManager.Instance.OpenMenu("MenuMain") as MenuMain;
		// 	}
		// 	else
		// 	{
		// 		menuMain.TestMove();
		// 	}
		// }
	}
}
