using System;

namespace AI
{
	public interface IStateBuilder<T, TParent> where T : AbstractState, new()
	{
		/// Create a child state with a specified handler type. The state will take the
		/// name of the handler type.
		IStateBuilder<NewStateT, IStateBuilder<T, TParent>> State<NewStateT>() where NewStateT : AbstractState, new();
	}
}

