using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public GameObject pfbCube;

	private float dist = 1f;

	private void Start()
	{
		int count = 2;
		for (int i = 0; i < count; ++i)
		{
			GameObject objCube = Instantiate(pfbCube);
			Vector3 pos = new Vector3(i * dist, i * dist, 0);
			objCube.transform.position = pos;
		}
	}
}
