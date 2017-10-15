using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// target：
// normal state - walk, idle
// walk state 
// change to work state 
public class HFSM_Test : MonoBehaviour 
{
	private AI.IState rootState;

	class NormalState : AI.State
	{
		public void OnUpdate()
		{
					
		}
	}

	class WalkState : AI.State
	{
		
	}

	class RunState : AI.State
	{
		
	}
}
