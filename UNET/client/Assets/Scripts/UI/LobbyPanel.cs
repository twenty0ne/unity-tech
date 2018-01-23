using UnityEngine;
using Tanks.Networking;

namespace Tanks.UI
{
	/// <summary>
	/// Lobby panel
	/// </summary>
	public class LobbyPanel : MonoBehaviour
	{
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

		public void OnBackClick()
		{
			Back();
		}

		private void Back()
		{
			m_NetManager.Disconnect();
			m_MenuUi.ShowDefaultPanel();
		}
	}
}