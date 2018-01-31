using UnityEngine.Networking;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using System.Text;
using System.Collections.Generic;

// IMPORT: 
public class MyNetworkConnection : NetworkConnection
{
    //  CSteamID steamId;
    string playerId;

    public MyNetworkConnection() : base()
    {
    }

    public MyNetworkConnection(string playerId)
    {
        //this.steamId = steamId;
        this.playerId = playerId;
    }

    public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
    {
        // if (playerId == SteamUser.GetSteamID().m_SteamID)
        //{
        //    // sending to self. short circuit
        //    TransportReceive(bytes, numBytes, channelId);
        //    error = 0;
        //    return true;
        //}
        //if (MyNetworkManager.instance.IsHost())
        //{
        //    TransportReceive(bytes, numBytes, channelId);
        //    error = 0;
        //    return true;
        //}

        // Send packet to peer through Steam
        bool ret = MyNetworkManager.instance.Send(bytes, numBytes, channelId);

        if (ret)
        {
            error = 0;
            return true;
        }
        else
        {
            error = 1;
            return false;
        }
    }

    public void CloseP2PSession()
    {
        //SteamNetworking.CloseP2PSessionWithUser(steamId);
        //steamId = CSteamID.Nil;
    }

}

