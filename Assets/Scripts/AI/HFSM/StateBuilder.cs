using System;

namespace AI
{
	public interface IStateBuilder<T, TParent> where T : AbstractState, new()
	{
		// Create a child state with a specified handler type. The state will take the
		// name of the handler type.
		IStateBuilder<NewStateT, IStateBuilder<T, TParent>> State<NewStateT>() where NewStateT : AbstractState, new();
	
		// Create a named child state with a specified handler type
		IStateBuilder<NewStateT, IStateBuilder<T, TParent>> State<NewStateT>(string name) where NewStateT : AbstractState, new();

		IStateBuilder<State, IStateBuilder<T, TParent>> State(string name);
		IStateBuilder<T, TParent> Enter(Action<T> onEnter);
		IStateBuilder<T, TParent> Exit(Action<T> onExit);
		IStateBuilder<T, TParent> Update(Action<T, float> onUpdate);
		IStateBuilder<T, TParent> Condition(Func<bool> predicate, Action<T> action);
		IStateBuilder<T, TParent> Event(string identifier, Action<T> action);
		IStateBuilder<T, TParent> Event<TEvent>(string identifier, Action<T, TEvent> action) where TEvent : EventArgs;

		// Finalise the current state and return the builder for its parent.
		TParent End();
	}

	public class StateBuilder<T, TParent> : IStateBuilder<T, TParent>
		where T : AbstractState, new()
	{
		private readonly TParent parentBuilder;
		private T state;

		// Create a new state builder with a specified parent state and parent builder.
		public StateBuilder(TParent parentBuilder, AbstractState parentState)
		{
			this.parentBuilder = parentBuilder;

			// New-up state of the prescrbed type.
			state = new T();
			parentState.AddChild(state);
		}
			
		// Create a new state builder with a specified parent state, parent builder,
		// and name for the new state.
		public StateBuilder(TParent parentBuilder, AbstractState parentState, string name)
		{
			this.parentBuilder = parentBuilder;

			// New-up state of the prescrbed type.
			state = new T();
			parentState.AddChild(state, name);
		}
			
		// Create a child state with a specified handler type. The state will take the
		// name of the handler type.
		public IStateBuilder<NewStateT, IStateBuilder<T, TParent>> State<NewStateT>() 
			where NewStateT : AbstractState, new()
		{
			return new StateBuilder<NewStateT, IStateBuilder<T, TParent>>(this, state);
		}
			
		// Create a named child state with a specified handler type.
		public IStateBuilder<NewStateT, IStateBuilder<T, TParent>> State<NewStateT>(string name) 
			where NewStateT : AbstractState, new()
		{
			return new StateBuilder<NewStateT, IStateBuilder<T, TParent>>(this, state, name);
		}
			
		// Create a child state with the default handler type.
		public IStateBuilder<State, IStateBuilder<T, TParent>> State(string name)
		{
			return new StateBuilder<State, IStateBuilder<T, TParent>>(this, state, name);
		}

		// <summary>
		// Set an action to be called when we enter the state.
		public IStateBuilder<T, TParent> Enter(Action<T> onEnter)
		{
			state.SetEnterAction(() => onEnter(state));

			return this;
		}
			
		// Set an action to be called when we exit the state.
		public IStateBuilder<T, TParent> Exit(Action<T> onExit)
		{
			state.SetExitAction(() => onExit(state));

			return this;
		}
			
		// Set an action to be called when we update the state.
		public IStateBuilder<T, TParent> Update(Action<T, float> onUpdate)
		{
			state.SetUpdateAction(dt => onUpdate(state, dt));

			return this;
		}
			
		// Set an action to be called on update when a condition is true.
		public IStateBuilder<T, TParent> Condition(Func<bool> predicate, Action<T> action)
		{
			state.SetCondition(predicate, () => action(state));

			return this;
		}
			
		// Set an action to be triggerable when an event with the specified name is raised.
		public IStateBuilder<T, TParent> Event(string identifier, Action<T> action)
		{
			state.SetEvent<EventArgs>(identifier, _ => action(state));

			return this;
		}
			
		// Set an action with arguments to be triggerable when an event with the specified name is raised.
		public IStateBuilder<T, TParent> Event<TEvent>(string identifier, Action<T, TEvent> action) 
			where TEvent : EventArgs
		{
			state.SetEvent<TEvent>(identifier, args => action(state, args));

			return this;
		}
			
		/// Finalise the current state and return the builder for its parent.
		public TParent End()
		{
			return parentBuilder;
		}
	}
}

