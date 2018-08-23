using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CmdType : byte
{
	// Operation 是与 Server 交互的
	OperationRequest,
	OperationResponse,
	// Host/Client 之间交互是通过 RPC
}

public class XNetCommand
{
	public CmdType type;
}

public class XNetOperationResponse
{
	public byte operationCode;
	public byte returnCode;
	public string message;
	// key - parameterCode
	public Dictionary<byte, object> parameters;
}
