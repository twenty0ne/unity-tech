using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class HttpRequest
{
	private const int BUFFER_SIZE = 1024;

	public static bool isKeepAlive = true;
	public static int timeout = 10000;	// 10s

	private class HttpState
	{
		public HttpWebRequest request;
		public byte[] data;
	}

	public static void Init()
	{
		
	}

	public static void Tick(float dt)
	{

	}

	public static void Post(string url, Dictionary<string, string> headers, byte[] data)
	{
		Request("POST", url, headers, data);
	}

	public static void Get(string url, Dictionary<string, string> headers = null)
	{
		Request("GET", url, headers, null);
	}

	public static void Request(string method, string url, Dictionary<string, string> headers, byte[] data)
	{
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

		if (headers != null)
		{
			var etor = headers.GetEnumerator();
			while (etor.MoveNext())
			{
				var kv = etor.Current;
				request.Headers[kv.Key] = kv.Value;
			}
		}

		request.KeepAlive = isKeepAlive;
		request.Timeout = timeout;
		request.Method = method;
		request.Proxy = null;

		HttpState state = new HttpState();
		state.request = request;
		state.data = data;

		request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), state);
	}

	private static void GetRequestStreamCallback(IAsyncResult asyncResult)
	{
		try
		{
			HttpState state = (HttpState)asyncResult.AsyncState;

			HttpWebRequest request = state.request;
			byte[] data = state.data;

			Stream reqStream = request.EndGetRequestStream(asyncResult);

			reqStream.Write(data, 0, data.Length);
			reqStream.Close();

			request.BeginGetResponse(new AsyncCallback(GetResponseCallback), state);
		}
		catch (WebException e)
		{
		}
	}

	private static void GetResponseCallback(IAsyncResult asyncResult)
	{
		try
		{
			HttpState state = (HttpState)asyncResult.AsyncState;

			HttpWebRequest request = state.request;
			HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
			Stream respStream = response.GetResponseStream();

			// stream read to bytes
			byte[] respData = null;
			using (MemoryStream memStream = new MemoryStream())
			{
				byte[] buffer = new byte[BUFFER_SIZE];

				int readBytes = 0;
				while ((readBytes = respStream.Read(buffer, 0, buffer.Length)) > 0)
				{
					memStream.Write(buffer, 0, readBytes);
				}
				respData = memStream.ToArray();
			}

			// ProcessResponse(response.Headers, respData);

			// Release the HttpWebResponse
			respStream.Close();
			// If use keep-alive, connect dont close
			// response.Close();
		}
		catch (WebException e)
		{
		}
	}
}
