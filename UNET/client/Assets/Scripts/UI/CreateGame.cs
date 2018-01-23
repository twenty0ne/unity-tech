using Tanks.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.UI
{
	/// <summary>
	/// Governs the Create Game functionality in the main menu.
	/// </summary>
	public class CreateGame : MonoBehaviour
	{
		[SerializeField]
		//Internal reference to the InputField used to enter the server name.
		protected InputField m_MatchNameInput;

		[SerializeField]
		//Internal reference to the MapSelect instance used to flip through multiplayer maps.
		protected MapSelect m_MapSelect;

		[SerializeField]
		//Internal reference to the ModeSelect instance used to cycle multiplayer modes.
		protected ModeSelect m_ModeSelect;

		//Cached references to other UI singletons.
		private MainMenuUI m_MenuUi;
#if XNET
        private XNetManager m_NetManager;
#else
        private NetworkManager m_NetManager;
#endif

		protected virtual void Start()
		{
			m_MenuUi = MainMenuUI.s_Instance;
#if XNET
            m_NetManager = XNetManager.instance;
#else
            m_NetManager = NetworkManager.s_Instance;
#endif
		}

		/// <summary>
		/// Back button method. Returns to main menu.
		/// </summary>
		public void OnBackClicked()
		{
			m_MenuUi.ShowDefaultPanel();
		}

		/// <summary>
		/// Create button method. Validates entered server name and launches game server.
		/// </summary>
		public void OnCreateClicked()
		{
#if XNET
#else
            if (string.IsNullOrEmpty(m_MatchNameInput.text))
			{
				m_MenuUi.ShowInfoPopup("Server name cannot be empty!", null);
				return;
			}
#endif

			StartMatchmakingGame();
		}

		/// <summary>
		/// Populates game settings for broadcast to clients and attempts to start matchmaking server session.
		/// </summary>
		private void StartMatchmakingGame()
		{
			GameSettings settings = GameSettings.s_Instance;
			settings.SetMapIndex(m_MapSelect.currentIndex);
			settings.SetModeIndex(m_ModeSelect.currentIndex);

			m_MenuUi.ShowConnectingModal(false);

			Debug.Log(GetGameName());
#if XNET
            m_NetManager.StartMatchmaking("0wI4g5Q8", (success, matchInfo) => {
                if (!success)
                {
                    m_MenuUi.ShowInfoPopup("Failed to create game.", null);
                }
                else
                {
                    m_MenuUi.HideInfoPopup();
                    m_MenuUi.ShowLobbyPanel();
                }
            });
#else
            m_NetManager.StartMatchmakingGame(GetGameName(), (success, matchInfo) =>
				{
					if (!success)
					{
						m_MenuUi.ShowInfoPopup("Failed to create game.", null);
					}
					else
					{
						m_MenuUi.HideInfoPopup();
						m_MenuUi.ShowLobbyPanel();
					}
				});
#endif
		}

		//Returns a formatted string containing server name and game mode information.
		private string GetGameName()
		{
			return string.Format("|{0}| {1}", m_ModeSelect.selectedMode.abbreviation, m_MatchNameInput.text);
		}
	}
}