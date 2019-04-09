using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class Tower : MonoBehaviour 
{
	[Range(10, 20)]
	public float rotateSpeed;
	public int stairNumOneFace = 6;

	// NOTE:
	// 不能设置 object 为 static, 否则无法 Instantiate
	public GameObject objStairCase;
	public GameObject objStairCorner;

	public Vector3 posStairStart;
	public Vector3[] stairStartPos;

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
		for (int i = 0; i < 50; ++i)
		{
			GameObject stairCase = Instantiate(objStairCase);
			stairCase.transform.SetParent(transform);

			int face = i / stairNumOneFace % 4;
			Debug.Log("xx-- face > " + face);
			if (face == 0)
			{
				stairCase.transform.position = stairStartPos[0] + 
						new Vector3(-i % stairNumOneFace * stairSize.x, i * stairSize.y, 0);
				// stairCase.transform.localRotation = Quaternion.Euler(0, 90, 0);
			}
			else if (face == 1)
			{
				stairCase.transform.position = stairStartPos[1] + 
						new Vector3(0, i * stairSize.y, i % stairNumOneFace * stairSize.x);
				stairCase.transform.localRotation = Quaternion.Euler(0, 90, 0);
			}
			else if (face == 2)
			{
				stairCase.transform.position = stairStartPos[2] + 
						new Vector3(i % stairNumOneFace * stairSize.x, i * stairSize.y, 0);
				stairCase.transform.localRotation = Quaternion.Euler(0, 180, 0);
			}
			else if (face == 3)
			{
				stairCase.transform.position = stairStartPos[3] + 
						new Vector3(0, i * stairSize.y, -i % stairNumOneFace * stairSize.x);
				stairCase.transform.localRotation = Quaternion.Euler(0, 270, 0);
			}
		}
	}
}
