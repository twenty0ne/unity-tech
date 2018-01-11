
using System.Collections.Generic;

class Stack
{
	private Queue<State> states = new Queue<State> ();
	private Dictionary<KeyValuePair<State, State>, System.Action> callbacks;

	Stack(State start)
	{
		states.Enqueue (start);

	}

	private bool Call(State from, State to)
	{
		KeyValuePair<State, State> kv = new KeyValuePair<State, State> (from, to);
		if (callbacks.ContainsKey(kv))
		{
			callbacks [kv] ();
			return true;
		}
		return false;
	}

	public System.Action On(State from, State to)
	{
		return callbacks [new KeyValuePair<State, State> (from, to)];
	}
}