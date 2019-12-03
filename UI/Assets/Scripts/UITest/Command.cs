using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandType
{
	public const ushort Login = 0;
}

public class Command
{
	public ushort ctype;
}

public class CmdLogin : Command
{
	public string name;
}