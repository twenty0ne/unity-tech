using UnityEngine;

public class MenuStart : UIMenu
{
	public static MenuStart Show()
	{
		UIMenu mu = UIManager.Instance.TryGetMenu(typeof(MenuStart));
		Debug.Assert(mu != null && mu.GetComponent<MenuStart>() != null, "CHECK");
		return mu.GetComponent<MenuStart>();
	}

	public static void Hide()
	{

	}
}