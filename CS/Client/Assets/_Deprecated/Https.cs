using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

public class Https : MonoBehaviour
{
	public void Test()
	{
		Debug.Log("start request > ");
		// StartCoroutine(Request());
		RequestSSL();
		// StartCoroutine(Request2());
	}

	// NOTE:
	// WWW 使用 https 会出错：ssl peer certificate or ssh remote key
	IEnumerator Request()
	{
		WWW www = new WWW("https://127.0.0.1:14711");
		yield return www;

		if (www.error != null)
		{
			Debug.Log("request error > " + www.error);
		}
		else
		{
			Debug.Log("request return > " + www.text);
		}
	}

	// https://docs.microsoft.com/en-us/dotnet/api/system.net.httpwebrequest.begingetresponse?redirectedfrom=MSDN&view=netframework-4.7.2#System_Net_HttpWebRequest_BeginGetResponse_System_AsyncCallback_System_Object_
	private HttpWebRequest webRequest = null;
	void RequestSSL()
	{
		ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;

		HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://127.0.0.1:14711");
		// HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		request.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);
		webRequest = request;

		//Stream dataStream = response.GetResponseStream();
		//StreamReader reader = new StreamReader(dataStream);
		//string responseFromServer = reader.ReadToEnd();

		//Debug.Log("responseFromServer=" + responseFromServer);
	}

	void FinishWebRequest(IAsyncResult result)
	{
		HttpWebResponse response = (HttpWebResponse)webRequest.EndGetResponse(result);

		Stream dataStream = response.GetResponseStream();
		StreamReader reader = new StreamReader(dataStream);
		string responseFromServer = reader.ReadToEnd();

		Debug.Log("responseFromServer=" + responseFromServer);
	}

	private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
	{
		// all Certificates are accepted
		return true;
	}

	// NOTE
	// Unknow error
	IEnumerator Request2()
	{
		UnityWebRequest www = new UnityWebRequest("https://127.0.0.1:14711");
		yield return www.Send() ;

		if (www.isError)
		{
			Debug.Log("request error > " + www.error);
		}
		else
		{
			Debug.Log("request return > " + www.downloadHandler.text);
		}
	}
}
