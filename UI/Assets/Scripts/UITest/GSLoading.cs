
public class GSLoading : GameState
{
	private MenuLoading _mloading = null;

	public override void Enter(GameState gs)
	{
		_mloading = (MenuLoading)UIManager.Instance.OpenMenu(MenuLoading.Name);
	}

	public override void Exit(GameState gs)
	{

	}

	public override void Tick()
	{
		
	}
}