using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class Tower : MonoBehaviour 
{
	[Range(10, 20)]
	public float rotateSpeed;

	// NOTE:
	// 不能设置 object 为 static, 否则无法 Instantiate
	public GameObject objStairCase;
	public GameObject objStairCorner;

	public Vector3 posStairStart;

	private Vector3 stairSize;

	private void Start() 
	{
		Debug.Assert(objStairCase != null, "CHECK");
		stairSize = objStairCase.GetComponent<Renderer>().bounds.size;
		Debug.Log("xx-- stairSize > " + stairSize);

		InitStairs();
	}

	private void Update() 
	{
		if (Input.GetKey(KeyCode.Space))
		{
			transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
			// transform.RotateAround(Vector3.up, rotateSpeed * Time.deltaTime);
			// transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
		}
	}

	private void InitStairs()
	{
		for (int i = 0; i < 10; ++i)
		{
			GameObject stairCase = Instantiate(objStairCase);
			stairCase.transform.SetParent(transform);
			stairCase.transform.position = posStairStart + 
					new Vector3(-i * stairSize.x, i * stairSize.y, 0);
		}
	}
}
