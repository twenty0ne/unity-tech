using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	private GameState _currGameState = null;
	private GameState _prevGameState = null;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void OnBeforeSceneLoadRuntimeMethod()
	{
		var gm = GameManager.Instance;
	}

	protected override void Awake()
	{
		base.Awake();

		var gs = new GSLoading();
		ChangeGameState(gs);
	}

	private void Update()
	{
		if (_currGameState != null)
		{
			_currGameState.Tick();
		}
	}

	public void ChangeGameState(GameState gs)
	{
		_prevGameState = _currGameState;
		if (_currGameState != null)
			_currGameState.Exit(gs);

		_currGameState = gs;
		_currGameState.Enter(gs);
	}
}
