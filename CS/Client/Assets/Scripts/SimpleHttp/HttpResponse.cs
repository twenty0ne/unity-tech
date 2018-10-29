using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class HttpResponse
{
	public HttpStatusCode statusCode;
	public Dictionary<string, string> headers;
	public byte[] data;
	public string errorMessage;
	public Exception errorException;
}
