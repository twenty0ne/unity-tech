using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveController : MonoBehaviour 
{
	public Camera camera; 

	private void Start()
	{

	}
	
	private void Update () 
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			var cpos = camera.transform.position;
			cpos.y += 0.1f;
			camera.transform.position = cpos;
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			var cpos = camera.transform.position;
			cpos.y -= 0.1f;
			camera.transform.position = cpos;
		}
	}
}
