using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class NetPlayer : MonoBehaviour
{
	//private Vector3 oldPos;
	private Vector3 newPos;
	//public float moveSpeed = 1f;
	//public float rotateSpeed = 1f;
	public float moveSmooth = 1f;
	public float rotateSmooth = 1f;

	//private float moveStartTime = 0f;
	//private float moveLength = 0;

	private bool isFromNet = false;

	private Quaternion newRotation = new Quaternion();

	public Animator animator = null;

	public struct NPOS
	{
		public float time;
		public Vector3 pos;
		public Quaternion rot;
	}

	// public Dictionary<float, Vector3> positionBuffer = new Dictionary<float, Vector3>();
	public List<NPOS> positionBuffer = new List<NPOS>();

	ThirdPersonCamera cam = null;

	private void Awake()
	{
		//oldPos = transform.position;
		newPos = transform.position;
		newRotation = transform.rotation;

		animator = GetComponent<Animator>();

		cam = GameObject.FindObjectOfType<ThirdPersonCamera>();
	}

	public void NewPos(Vector3 npos, Vector3 nrot)
	{
		isFromNet = true;

		//oldPos = transform.position;
		// newPos = npos;
		// newRotation.eulerAngles = nrot;

		//moveStartTime = Time.time;
		//moveLength = Vector3.Distance(oldPos, newPos);
		var pos = new NPOS();
		pos.time = Time.time;
		// pos.pos = new Vector3(npos.x, transform.position.y, npos.z);
		pos.pos = npos;
		pos.rot = new Quaternion();
		pos.rot.eulerAngles = nrot;
		positionBuffer.Add(pos);
	}

	// private void LateUpdate()
	private void Update()
	{
		if (!isFromNet)
			return;
		if (positionBuffer.Count < 2)
			return;
		while (positionBuffer.Count > 2)
		{
			positionBuffer.RemoveAt(0);
		}

		// if (moveLength <= 0f)
		//		return;

		// float distMoved = (Time.time - moveStartTime) * moveSpeed;
		// float fracMoved = distMoved / moveLength;
		if (positionBuffer.Count >= 2)
		{
			Vector3 op = positionBuffer[0].pos;
			Vector3 np = positionBuffer[1].pos;
			Quaternion or =  positionBuffer[0].rot;
			Quaternion nr = positionBuffer[1].rot;

			float dt = (Time.time - 0.1f - positionBuffer[0].time) / (positionBuffer[1].time - positionBuffer[0].time);

			transform.position = Vector3.Lerp(op, np, dt);
			transform.rotation = Quaternion.Lerp(or, nr, dt);
			// transform.position = Vector3.Lerp(op, np, Time.deltaTime * moveSmooth);
			// transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotateSmooth);
		}
	}
}
*/
 
public class NetPlayer
{
	public string id;
}