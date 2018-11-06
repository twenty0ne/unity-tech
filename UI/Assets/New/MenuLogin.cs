using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogin : UIMenu
{
    public InputField inputName;

    public void OnClickBtnLogin()
    {
        Debug.Log("MenuLogin.OnClickBtnLogin");

        if (string.IsNullOrEmpty(inputName.text))
        {
            DialogConfirm dlgConfirm = (DialogConfirm)UIManager.Instance.OpenDialog("DialogConfirm");
            dlgConfirm.title = "WARNING";
            dlgConfirm.text = "input name shouldn't be empty";
            dlgConfirm.btnText = "OK";
            dlgConfirm.evtClose += (UIPanel panel) =>
            {
                Debug.Log("close dialog confirm callback");
            };
        }
        else
        {
            UIManager.Instance.OpenMenu("MenuMain");
        }
    }
}
