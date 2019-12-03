using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalServer : Singleton<LocalServer>
{
	public void ReceiveCommand(Command cmd)
	{
		ushort ctype = cmd.ctype;
		
		if (ctype == CommandType.Login)
			OnCmdLogin((CmdLogin)cmd);
	}

	private void OnCmdLogin(CmdLogin cmd)
	{
		// DataManager.Instance.
	}
}
