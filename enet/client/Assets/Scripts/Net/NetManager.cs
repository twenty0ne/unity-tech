using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ChanId : byte
{
	POS,
}

// NetManager 是 PVP Net 底层与游戏逻辑之间的连接
public class NetManager : MonoBehaviour
{
	NetClient nclient = new NetClient();

	public bool isHost = false;
	public GameObject player = null;

	private int lastFrameCount = 0;

	private void Start()
	{
		nclient.Connect("127.0.0.1", 17210);
	}

	private void Update()
	{
		TickS2C();
		TickC2S();
	}

	private void TickS2C()
	{
		nclient.ReceivePackets(ParsePacket);
	}

	private void TickC2S()
	{
		if (isHost == false)
			return;

		//if (Time.frameCount - lastFrameCount < 30)
		//	return;
		//lastFrameCount = Time.frameCount;

		SendPostions();
	}

	private void SendPostions()
	{
		if (player == null)
			return;

		// 封装 postion 数据
		NetBuffer buffer = new NetBuffer();

		buffer.Write((byte)NetField.POS);
		Vector3 pos = player.transform.position;
		buffer.Write(pos);

		buffer.Write((byte)NetField.ROTATION);
		Vector3 rot = player.transform.eulerAngles;
		buffer.Write(rot);

		ENet.Packet pkt = new ENet.Packet();
		pkt.Create(buffer.data, 0, buffer.length, ENet.PacketFlags.NoAllocate);

		nclient.SendClientPacket(pkt, (byte)ChanId.POS);
		Debug.Log("xx-- send postions > " + pos.ToString());
	}

	private void SendMessages()
	{

	}

	private void ReceivePositions(NetBuffer buffer)
	{
		NetField field = (NetField)buffer.ReadByte();
		if (field == NetField.POS)
		{
			Vector3 pos = buffer.ReadVector3();
			if (player != null)
			{
				// Debug.Log("xx-- receive positons > " + pos.ToString());
				player.transform.position = pos;
			}
		}

		field = (NetField)buffer.ReadByte();
		if (field == NetField.ROTATION)
		{
			Vector3 rot = buffer.ReadVector3();
			if (player != null)
				player.transform.eulerAngles = rot;
		}
	}

	private void ParsePacket(byte chan, ENet.Packet pkt)
	{
		NetBuffer buffer = new NetBuffer();
		buffer.data = pkt.GetBytes();

		ChanId chanId = (ChanId)chan;
		if (chanId == ChanId.POS)
			ReceivePositions(buffer);
	}
}
