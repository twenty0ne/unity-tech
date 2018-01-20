using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lidgren.Network;

public class XNetManager : MonoBehaviour 
{
	// C2H = client to host
	// H2C = host to client
	public enum DataType
	{
		C2H_Connect = 0,
		H2C_Accept,
		SYNC,
	}

	public static XNetClient instance = null;

	private NetClient netClient = null;

	private void Awake()
	{
		if (instance != null) 
		{
			Debug.LogError ("More than one XNetManager instance was found.");
			this.enabled = false;
			return;
		}

		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;

		if (netClient != null) 
		{
			netClient.Disconnect ();
			netClient = null;
		}
	}

	private void Update()
	{
		if (netClient != null) 
		{
			this.PeekMessages ();
		}
	}

	private void OnConnected()
	{
		
	}

	private void OnDisconnected()
	{
		
	}

	private void OnData(NetIncomingMessage im)
	{
		int dataType = im.ReadInt32();
		if (dataType == DataType.C2H_Connect)
		{
			string clientId = im.ReadString();

			var newConn = new XNetConnection();
			newConn.ForceInitialize();

			NetworkServer.AddExternalConnection(newConn);
			connRemote = newConn;
			//});

			// send accept
			NetOutgoingMessage om = _netClient.CreateMessage();
			om.Write(DATA_TYPE.DAT_ACCEPT);
			om.Write(client_id);
			_netClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 0);
		}
		else if (dataType == DataType.H2C_Accept)
		{
			// Create connection to host player's steam ID
			var conn = new MyNetworkConnection();
			var mySteamClient = new MyNetworkClient(conn);
			myClient = mySteamClient;

			// Setup and connect
			mySteamClient.RegisterHandler(MsgType.Connect, OnConnect);
			mySteamClient.SetNetworkConnectionClass<MyNetworkConnection>();
			mySteamClient.Configure(MyNetworkManager.hostTopology);
			mySteamClient.Connect();
		}
		else if (dataType == DataType.SYNC)
		{
			int byteLength = im.ReadInt32();
			byte[] byteArray = im.ReadBytes(byteLength);

			NetworkConnection conn = isHost ? connRemote : myClient.connection;

			if (conn != null)
			{
				conn.TransportReceive(byteArray, byteLength, 0);
			}
		}
	}

	private void PeekMessages()
	{
		NetIncomingMessage im;
		while ((im = netClient.ReadMessage()) != null)
		{
			// handle incoming message
			switch (im.MessageType)
			{
			case NetIncomingMessageType.DebugMessage:
			case NetIncomingMessageType.ErrorMessage:
			case NetIncomingMessageType.WarningMessage:
			case NetIncomingMessageType.VerboseDebugMessage:
				{
					string text = im.ReadString ();
					Debug.Log ("output: " + text);
				}
				break;
			case NetIncomingMessageType.StatusChanged:
				{
					NetConnectionStatus status = (NetConnectionStatus)im.ReadByte ();

					string reason = im.ReadString ();
					Debug.Log (status.ToString () + ": " + reason);

					if (status == NetConnectionStatus.Connected)
						this.OnConnected ();
					else if (status == NetConnectionStatus.Disconnected)
						this.OnDisconnected ();
				}
				break;
			case NetIncomingMessageType.Data:
				{
					this.OnData (im);
				}
				break;
			default:
				Debug.Log("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
				break;
			}
			netClient.Recycle(im);
		}
	}
}
