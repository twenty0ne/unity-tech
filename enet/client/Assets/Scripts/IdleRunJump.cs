using UnityEngine;
using System.Collections;

public class IdleRunJump : MonoBehaviour {


	protected Animator animator;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = true;

	public float nSpeed = 0f;
	public float nDirection = 0f;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
		
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);
	}
		
	// Update is called once per frame
	void Update () 
	{
		if (!animator)
			return;

		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		if (NetManager.Instance.isHost)
		{
			animator.applyRootMotion = true;

			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetButton("Fire1"))
				{
					animator.SetBool("Jump", true);

					NetManager.Instance.AddMessage(NetField.JUMP);
				}
			}
			else
			{
				animator.SetBool("Jump", false);
			}

			if (Input.GetButtonDown("Fire2") && animator.layerCount >= 2)
			{
				animator.SetBool("Hi", !animator.GetBool("Hi"));
			}

			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			nSpeed = h * h + v * v;
			nDirection = h;

			NetManager.Instance.AddMessage(NetField.ANI, h * h + v * v, h);
		}
		else
		{
			animator.applyRootMotion = false;

			if (stateInfo.IsName("Base Layer.Run"))
			{

			}
			else
			{
				animator.SetBool("Jump", false);
			}
		}

		animator.SetFloat("Speed", nSpeed);
		animator.SetFloat("Direction", nDirection, DirectionDampTime, Time.deltaTime);
	}
}
