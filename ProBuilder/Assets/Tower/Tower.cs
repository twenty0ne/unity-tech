using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
	[Range(10, 20)]
	public float rotateSpeed;

	public GameObject objStairCase;
	public GameObject objStairCorner;

	public Vector3 stairStartPos;

	private float stairHeight;

	private void Start() 
	{
		Debug.Assert(objStairCase != null, "CHECK");
		stairHeight = objStairCase.GetComponent<Renderer>().bounds.size.y;
		Debug.Log("xx-- stairHeight > " + stairHeight);

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

	}
}
