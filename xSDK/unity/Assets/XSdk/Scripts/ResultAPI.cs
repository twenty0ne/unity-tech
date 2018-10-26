using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

namespace XSDK
{
	public class ResultAPI
	{
		public enum Code : int
		{
			SUCCESS = 0,
		}

		public enum ErrorCode : int
		{
			SUCCESS = 0,
		}

		public ErrorCode errorCode = ErrorCode.SUCCESS;
		public Code code = Code.SUCCESS;
		public string errorMessage = "SUCCESS";
		public string message = "SUCCESS";

		public ResultAPI(JsonObject resJsonParam)
		{

		}
	}
}


