// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class MenuLogin : UIMenu
// {
// 	public InputField inputName;

// 	public void OnClickBtnLogin()
// 	{
// 		Debug.Log("MenuLogin.OnClickBtnLogin");

// 		if (string.IsNullOrEmpty(inputName.text))
// 		{
// 			DialogConfirm dlgConfirm = (DialogConfirm)UIManager.Instance.OpenDialog("DialogConfirm");
// 			dlgConfirm.title = "WARNING";
// 			dlgConfirm.text = "input name shouldn't be empty";
// 			dlgConfirm.btnText = "OK";
// 			dlgConfirm.onClose += (UIPanel panel) =>
// 			{
// 				Debug.Log("dialog close > name empty warning");
// 			};

// 			// Simulate open two same Dialog
// //			DialogConfirm dlgWarning = (DialogConfirm)UIManager.Instance.OpenDialog("DialogConfirm");
// //			dlgWarning.title = "WARNING";
// //			dlgWarning.text = "network error";
// //			dlgWarning.btnText = "OK";
// //			dlgWarning.evtClose += (UIPanel panel) =>
// //			{
// //				Debug.Log("dialog close > network error");
// //			};
// 		}
// 		else
// 		{
// 			UIManager.Instance.OpenMenu("MenuMain");
// 		}
// 	}
// }
