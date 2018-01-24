using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using Lidgren.Network;

using SamplesCommon;

namespace ChatServer
{
    public class DataType
    {
        public const int MATCH = 0;
        public const int C2H_Connect = 1;
        public const int H2C_Accept = 2;
        public const int Sync = 3;
        public const int GetMatchList = 4;
        public const int JoinMatch = 5;
        public const int JoinMatchReturn = 6;
    }

    public class NetCode
    {
        public const int OK = 0;
        // match
        public const int MATCH_ROOMCREATE_SUCCESS = 100;
        public const int MATCH_ROOMCREATE_FAIL = 101;
        public const int MATCH_ROOMJOIN_SUCCESS = 102;
        public const int MATCH_ROOMJOIN_FAIL = 103;
    }

    static class Program
	{
        private const int MAX_CONNECTIONS = 4;

		private static Form1 s_form;
		private static NetServer s_server;
		private static NetPeerSettingsWindow s_settingsWindow;

        private class Room
        {
            public string name;
            //public int timeCreate;

            //public string ipHost;
            //public int portHost;

            //public string ipClient;
            //public int portClient;
            public NetConnection host;
            // public NetConnection client;
            public List<NetConnection> conns = new List<NetConnection>();
        }

        private static Dictionary<string, Room> s_rooms = new Dictionary<string, Room>();
        private static Dictionary<string, string> s_refRooms = new Dictionary<string, string>();
		
		[STAThread]
		static void Main()
	{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			s_form = new Form1();

			// set up network
			NetPeerConfiguration config = new NetPeerConfiguration("pvp");
			config.MaximumConnections = 4;
			config.Port = 14242;
			s_server = new NetServer(config);

			Application.Idle += new EventHandler(Application_Idle);
			Application.Run(s_form);
		}

		private static void Output(string text)
		{
			NativeMethods.AppendText(s_form.richTextBox1, text);
		}

		private static void Application_Idle(object sender, EventArgs e)
		{
			while (NativeMethods.AppStillIdle)
			{
				NetIncomingMessage im;
				while ((im = s_server.ReadMessage()) != null)
				{
					// handle incoming message
					switch (im.MessageType)
					{
						case NetIncomingMessageType.DebugMessage:
						case NetIncomingMessageType.ErrorMessage:
						case NetIncomingMessageType.WarningMessage:
						case NetIncomingMessageType.VerboseDebugMessage:
							string text = im.ReadString();
							Output(text);
							break;

						case NetIncomingMessageType.StatusChanged:
							NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

							string reason = im.ReadString();
							Output(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);

                            if (status == NetConnectionStatus.Connected)
                            {
                                //string roomId = im.SenderConnection.RemoteHailMessage.ReadString();
                                //Output("Remote hail: roomId>" + roomId);

                                //// create host
                                //if (s_rooms.ContainsKey(roomId) == false)
                                //{
                                //    Room room = new Room();
                                //    room.id = roomId;
                                //    room.host = im.SenderConnection;
                                //    s_rooms[room.id] = room;

                                //    s_refRooms[im.SenderConnection.RemoteUniqueIdentifier.ToString()] = room.id;
                                //}
                                //// create client
                                //else 
                                //{
                                //    Room room = s_rooms[roomId];
                                //    room.client = im.SenderConnection;

                                //    s_refRooms[im.SenderConnection.RemoteUniqueIdentifier.ToString()] = room.id;

                                //    // send connect to host
                                //    NetOutgoingMessage om = s_server.CreateMessage(im.Data.Length);
                                //    om.Write(DATA_TYPE.DAT_CONNECT);
                                //    om.Write(im.SenderConnection.RemoteUniqueIdentifier.ToString());
                                //    s_server.SendMessage(om, room.host, NetDeliveryMethod.ReliableOrdered, 0);
                                //}
                            }
                            else if (status == NetConnectionStatus.Disconnected)
                            {
                                string clientId = im.SenderConnection.RemoteUniqueIdentifier.ToString();
                                if (s_refRooms.ContainsKey(clientId))
                                {
                                    string roomName = s_refRooms[clientId];
                                    if (roomName != null && s_rooms.ContainsKey(roomName))
                                    {
                                        Room room = s_rooms[roomName];

                                        if (clientId == room.host.RemoteUniqueIdentifier.ToString())
                                        {
                                            s_rooms.Remove(roomName);
                                        }
                                        else
                                        {
                                            for (int i = 0; i < room.conns.Count; ++i)
                                            {
                                                NetConnection conn = room.conns[i];
                                                if (conn.RemoteUniqueIdentifier.ToString() == clientId)
                                                {
                                                    room.conns.Remove(conn);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

							UpdateConnectionsList();
							break;
						case NetIncomingMessageType.Data:
                            // incoming chat message from a client
                            string client_id = im.SenderConnection.RemoteUniqueIdentifier.ToString();

                            int dataType = im.ReadInt32();
                            if (dataType == DataType.MATCH)
                            {
                                string roomName = im.ReadString();
                                if (s_rooms.ContainsKey(roomName) == false)
                                {
                                    Room room = new Room();
                                    s_rooms[roomName] = room;

                                    room.name = roomName;
                                    room.host = im.SenderConnection;
                                    room.conns.Add(im.SenderConnection);

                                    NetOutgoingMessage om = s_server.CreateMessage();
                                    om.Write(DataType.MATCH);
                                    om.Write(NetCode.MATCH_ROOMCREATE_SUCCESS);
                                    s_server.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

                                    s_refRooms[client_id] = roomName;
                                }
                                else
                                {
                                    Room room = s_rooms[roomName];
                                    if (room == null || room.conns.Count >= MAX_CONNECTIONS)
                                    {
                                        NetOutgoingMessage om = s_server.CreateMessage();
                                        om.Write(DataType.MATCH);
                                        om.Write(NetCode.MATCH_ROOMJOIN_FAIL);
                                        s_server.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                                    else
                                    {
                                        room.conns.Add(im.SenderConnection);

                                        NetOutgoingMessage om = s_server.CreateMessage();
                                        om.Write(DataType.MATCH);
                                        om.Write(NetCode.MATCH_ROOMJOIN_SUCCESS);
                                        s_server.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

                                        s_refRooms[client_id] = roomName;
                                    }
                                }
                            }
                            else if (dataType == DataType.GetMatchList)
                            {
                                NetOutgoingMessage om = s_server.CreateMessage();
                                om.Write(DataType.GetMatchList);

                                om.Write(s_rooms.Count);
                                foreach (var room in s_rooms.Values)
                                {
                                    om.Write(room.name);
                                    om.Write(room.host.RemoteUniqueIdentifier.ToString() + "|" + room.name);
                                    om.Write(MAX_CONNECTIONS);
                                    om.Write(room.conns.Count);
                                }
                                s_server.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                            }
                            else if (dataType == DataType.JoinMatch)
                            {
                                string roomName = im.ReadString();
                                
                                if (s_rooms.ContainsKey(roomName))
                                {
                                    //NetOutgoingMessage om = s_server.CreateMessage(im.Data.Length);
                                    //om.Write(DATA_TYPE.DAT_CONNECT);
                                    //om.Write(im.SenderConnection.RemoteUniqueIdentifier.ToString());
                                    //s_server.SendMessage(om, room.host, NetDeliveryMethod.ReliableOrdered, 0);

                                    Room room = s_rooms[roomName];
                                    room.conns.Add(im.SenderConnection);
                                    s_refRooms[client_id] = roomName;

                                    NetOutgoingMessage om = s_server.CreateMessage();
                                    om.Write(DataType.JoinMatchReturn);
                                    om.Write(true);
                                    om.Write(client_id);
                                    s_server.SendMessage(om, room.conns, NetDeliveryMethod.ReliableOrdered, 0);
                                }
                                else
                                {
                                    NetOutgoingMessage om = s_server.CreateMessage();
                                    om.Write((Int32)DataType.JoinMatchReturn);
                                    om.Write(false);
                                    om.Write(client_id);
                                    s_server.SendMessage(om, im.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                                }
                            }
                            else if (dataType == DataType.Sync)
                            {
                                if (s_refRooms.ContainsKey(client_id))
                                {
                                    Room room = s_rooms[s_refRooms[client_id]];
                                    if (room != null)
                                    {
                                        List<NetConnection> all = new List<NetConnection>(room.conns);  // get copy
                                        all.Remove(im.SenderConnection);

                                        if (all.Count > 0)
                                        {
                                            NetOutgoingMessage om = s_server.CreateMessage();
                                            om.Write(im.Data);
                                            s_server.SendMessage(om, all, NetDeliveryMethod.Unreliable, 0);
                                        }
                                    }
                                }
                            }

                            //string remote_id = im.SenderConnection.RemoteUniqueIdentifier.ToString();
                            //if (s_refRooms.ContainsKey(remote_id))
                            //{
                            //    string room_id = s_refRooms[remote_id];
                            //    Room room = s_rooms[room_id];

                            //    NetOutgoingMessage om = s_server.CreateMessage(im.Data.Length);
                            //    om.Write(im.Data);
                            //    if (remote_id == room.host.RemoteUniqueIdentifier.ToString() && room.client != null)
                            //    {
                            //        s_server.SendMessage(om, room.client, NetDeliveryMethod.ReliableOrdered, 0);
                            //    }
                            //    else if (room.client != null && remote_id == room.client.RemoteUniqueIdentifier.ToString())
                            //    {
                            //        s_server.SendMessage(om, room.host, NetDeliveryMethod.ReliableOrdered, 0);
                            //    }
                            //}
                            //else
                            //{
                            //    Output("ERROR: cant find room for remote id>" + remote_id);
                            //}

                            /*
							string chat = im.ReadString();

							Output("Broadcasting '" + chat + "'");

							// broadcast this to all connections, except sender
							List<NetConnection> all = s_server.Connections; // get copy
							all.Remove(im.SenderConnection);

							if (all.Count > 0)
							{
								NetOutgoingMessage om = s_server.CreateMessage();
								om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
								s_server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
							}
                            */
                            break;
						default:
							Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
							break;
					}
					s_server.Recycle(im);
				}
				Thread.Sleep(1);
			}
		}

		private static void UpdateConnectionsList()
		{
			s_form.listBox1.Items.Clear();

			foreach (NetConnection conn in s_server.Connections)
			{
				string str = NetUtility.ToHexString(conn.RemoteUniqueIdentifier) + " from " + conn.RemoteEndPoint.ToString() + " [" + conn.Status + "]";
				s_form.listBox1.Items.Add(str);
			}
		}

		// called by the UI
		public static void StartServer()
		{
			s_server.Start();
		}

		// called by the UI
		public static void Shutdown()
		{
			s_server.Shutdown("Requested by user");
		}

		// called by the UI
		public static void DisplaySettings()
		{
			if (s_settingsWindow != null && s_settingsWindow.Visible)
			{
				s_settingsWindow.Hide();
			}
			else
			{
				if (s_settingsWindow == null || s_settingsWindow.IsDisposed)
					s_settingsWindow = new NetPeerSettingsWindow("Chat server settings", s_server);
				s_settingsWindow.Show();
			}
		}
	}
}
