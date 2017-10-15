using System.Collections;
using System.Collections.Generic;

namespace AI
{
	public class StateMachineBuilder 
	{
		private readonly State rootState;

		public StateMachineBuilder()
		{
			rootState = new State();
		}
			
		// Create a new state of a specified type and add it as a child of the root state.
		public IStateBuilder<T, StateMachineBuilder> State<T>() where T : AbstractState, new()
		{
			return new StateBuilder<T, StateMachineBuilder>(this, rootState);
		}
			
		// Create a new state of a specified type with a specified name and add it as a
		// child of the root state.
		public IStateBuilder<T, StateMachineBuilder> State<T>(string stateName) where T : AbstractState, new()
		{
			return new StateBuilder<T, StateMachineBuilder>(this, rootState, stateName);
		}
			
		// Create a new state with a specified name and add it as a
		// child of the root state.
		public IStateBuilder<State, StateMachineBuilder> State(string stateName)
		{
			return new StateBuilder<State, StateMachineBuilder>(this, rootState, stateName);
		}
			
		public IState Build()
		{
			return rootState;
		}
	}
}

