using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class ENetTransport : XNetTransport
{
	private ENet.Host host;
	private ENet.Peer epeer;

	public override void Connect(string address, int port)
	{
		base.Connect(address, port);

		host = new ENet.Host();
		host.Create(null, 1);

		ENet.Address addr = new ENet.Address();
		addr.SetHost(address);
		addr.Port = (ushort)port;

		epeer = host.Connect(addr, 200, 1234);

		Thread th = new Thread(PeekPackets);
		th.Start();
	}

	public override void Send(byte[] data, int length)
	{
		base.Send(data, length);


	}

	private void PeekPackets()
	{
		// TODO
		// how to set timeout
		while (host.Service(1) >= 0)
		{
			ENet.Event evt;
			while (host.CheckEvents(out evt) > 0)
			{
				switch (evt.Type)
				{
					case ENet.EventType.Connect:
						{
							peer.OnConnect();
						}
						break;
					case ENet.EventType.Disconnect:
						{
							peer.OnDisconnect();
						}
						break;
					case ENet.EventType.Receive:
						{
							var data = evt.Packet.GetBytes();
							peer.ReceiveData(data, data.Length);
							evt.Packet.Dispose();
						}
						break;
				}
			}
		}
	}
}
