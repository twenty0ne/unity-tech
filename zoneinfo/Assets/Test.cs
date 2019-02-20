using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Test : MonoBehaviour {

	public Text txt;

	// Use this for initialization
	void Start ()
	{
		//RemoveTZDataUselessLine();
		//return;

		int tsoffset = -27271131;
		PublicDomain.ZoneInfo.Zone zz = null;

		try
		{
			PublicDomain.ZoneInfo.Database.Reset()
			
			PublicDomain.ZoneInfo.Database.LoadUnityResources("tzdata");

			zz = PublicDomain.ZoneInfo.Database.GetZone("America/New_York");
			
			// cant reset, or dont get dailylight data
			// PublicDomain.ZoneInfo.Database.Reset();
		}
		catch (Exception ex)
		{
			Debug.LogError("Error: " + ex.ToString());
		}

		if (zz != null)
		{
			var d2 = DateTime.UtcNow.AddSeconds(-tsoffset);
			var d3 = zz.ConvertToLocal(d2);

			txt.text = d3.ToString();
		}
	}
	
	public void RemoveTZDataUselessLine()
	{
		TextAsset[] txts = Resources.LoadAll<TextAsset>("tzdata");
		if (txts != null)
		{
			for (int i = 0; i < txts.Length; ++i)
			{
				using (StringReader reader = new StringReader(txts[i].text))
				{
					using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/" + txts[i].name + ".txt.copy"))
					{
						string line;
						while ((line = reader.ReadLine()) != null)
						{
							if (line.Trim().StartsWith("#"))
								continue;

							// Ignore empty lines
							if (string.IsNullOrEmpty(line.Trim()))
								continue;

							writer.WriteLine(line);
						}
					}
				}
			}
		}

		Debug.Log("finish RemoveTZDataUselessLine");
	}
}
