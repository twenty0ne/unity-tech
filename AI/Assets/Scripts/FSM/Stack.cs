
using System.Collections.Generic;

class Stack
{
	private Queue<State> states = new Queue<State> ();

	Stack(State start)
	{
		states.Enqueue (start);

	}

	private bool Call(State from, State to)
	{
		
	}
}