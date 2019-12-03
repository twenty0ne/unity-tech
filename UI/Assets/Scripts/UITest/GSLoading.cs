
public class GSLoading : GameState
{
	private MenuLoading _mloading = null;
	private LoadingStep _loadingStep = LoadingStep.None;

	public override void Enter(GameState gs)
	{
		_mloading = (MenuLoading)UIManager.Instance.OpenMenu(MenuLoading.Name);
	}

	public override void Exit(GameState gs)
	{
		
	}

	public override void Tick()
	{
		if (_loadingStep == LoadingStep.None)
			ChangeLoadingStep(_loadingStep + 1);
		
		if (_loadingStep >= LoadingStep.Count)
		{

		}
	}

	private void ChangeLoadingStep(LoadingStep step)
	{
		if (_loadingStep == step)
		{
			Logger.Warning("CHECK: set same loading step.");
			return;
		}

		_loadingStep = step;

		if (_loadingStep == LoadingStep.Login)
			OnLoadingStepLogin();
		else if (_loadingStep == LoadingStep.Data)
			OnLoadingStepData();
		else if (_loadingStep == LoadingStep.Main)
			OnLoadingStepMain();
		else if (_loadingStep == LoadingStep.Count)
			OnLoadingStepEnd();

		EventManager.Instance.Dispatch(EventName.LoadingStep, (int)_loadingStep);
	}

	private void OnLoadingStepLogin()
	{

	}

	private void OnLoadingStepData()
	{

	}

	private void OnLoadingStepMain()
	{

	}

	private void OnLoadingStepEnd()
	{
		EventManager.Instance.Dispatch(EventName.LoadingEnd);
	}
}