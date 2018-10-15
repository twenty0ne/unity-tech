using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Https : MonoBehaviour
{
	public void Test()
	{
		Debug.Log("start request > ");
		// StartCoroutine(Request());
		StartCoroutine(RequestSSL());
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

	IEnumerator RequestSSL()
	{
		ServicePointManager.ServerCertificateValidationCallback = TrustCertificate;

		HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://127.0.0.1:14711");
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();

		Stream dataStream = response.GetResponseStream();
		StreamReader reader = new StreamReader(dataStream);
		string responseFromServer = reader.ReadToEnd();

		Debug.Log("responseFromServer=" + responseFromServer);

		yield return 0;
	}

	private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
	{
		// all Certificates are accepted
		return true;
	}
}
