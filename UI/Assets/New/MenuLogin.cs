using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogin : UIMenu
{
    public InputField inputName;

    public void OnClickBtnLogin()
    {
        // if (string.IsNullOrEmpty(inputName.text))
        {
            UIManager.Instance.OpenDialog("DialogConfirm");
        }


    }
}
