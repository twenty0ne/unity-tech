//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MyNetManager : MonoBehaviour
//{
//    public static MyNetManager instance = null;

//    private void Awake()
//    {
//        if (instance != null)
//        {
//            Debug.LogError("More than one XNetManager instance was found.");
//            this.enabled = false;
//            return;
//        }

//        instance = this;
//        DontDestroyOnLoad(gameObject);

//        XNetManager.Instance.Init();
//    }

//    private void Update()
//    {
//        XNetManager.Instance.Update();
//    }

//    public void Disconnect()
//    {
//    }

//    #region MatchMaking

//    public void StartMatchmaking(string roomName, System.Action<bool, XNetMatchInfo> onMatch)
//    {
//        if (XNetManager.Instance.IsConnectServer() == false)
//        {
//            XNetManager.Instance.ConnectServer("127.0.0.1", 14242, (success)=>
//            {
//                // TODO
//                // refactor > user shouldn't know NetOutgoingMessage exit
//                //XNetMessage msg = new XNetMessage();
//                //msg.Write(XNetManager.DataType.MATCH);
//                //msg.Write(roomName);

//                Lidgren.Network.NetClient netClient = XNetManager.Instance.GetNetClient();
//                Lidgren.Network.NetOutgoingMessage om = netClient.CreateMessage();
//                om.Write(XNetManager.DataType.MATCH);
//                om.Write(roomName);
//                netClient.SendMessage(om, Lidgren.Network.NetDeliveryMethod.ReliableOrdered, 0);

//                // XNetManager.Instance.SendMessage(msg);

//                //XNetMatchInfo matchInfo = new XNetMatchInfo();
//                //onMatch(true, matchInfo);
//            });
//        }
//        else
//        {
//            XNetMatchInfo matchInfo = new XNetMatchInfo();
//            onMatch(true, matchInfo);
//        }
//    }
//    #endregion
//}
