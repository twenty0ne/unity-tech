using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ChanId : byte
{
	POS,
	MESSAGE,
}

// NetManager 是 PVP Net 底层与游戏逻辑之间的连接
public class NetManager : MonoBehaviour
{
	public static NetManager Instance = null;

	NetClient nclient = new NetClient();

	public bool isHost = false;
	public NetPlayer nplayer = null;

	private float lastTickC2STime = 0;

	// public List<NetBuffer> messages = new List<NetBuffer>();
	NetBuffer messages = null;

	private void Awake()
	{
		Instance = this;
	}

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

		if (Time.time - lastTickC2STime < 0.1f)
			return;
		lastTickC2STime = Time.time;

		SendPostions();
		SendMessages();
	}

	private void SendPostions()
	{
		if (nplayer == null)
			return;

		// 封装 postion 数据
		NetBuffer buffer = new NetBuffer();

		buffer.Write((byte)NetField.POS);
		Vector3 pos = nplayer.transform.position;
		buffer.Write(pos);

		Vector3 rot = nplayer.transform.rotation.eulerAngles;
		buffer.Write(rot);

		ENet.Packet pkt = new ENet.Packet();
		pkt.Create(buffer.data, 0, buffer.length, ENet.PacketFlags.NoAllocate);

		nclient.SendPacket(pkt, (byte)ChanId.POS);
		// Debug.Log("xx-- send postions > " + pos.ToString());
	}

	private void SendMessages()
	{
		if (messages == null)
			return;

		ENet.Packet pkt = new ENet.Packet();
		pkt.Create(messages.data, 0, messages.length, ENet.PacketFlags.NoAllocate);
		messages = null;

		nclient.SendPacket(pkt, (byte)ChanId.MESSAGE);
		Debug.Log("xx-- send messages");
	}

	private void ReceivePositions(NetBuffer buffer)
	{
		while (buffer.IsEndOfRead() == false)
		{
			NetField field = (NetField)buffer.ReadByte();
			if (field == NetField.POS)
			{
				Vector3 pos = buffer.ReadVector3();
				Vector3 rot = buffer.ReadVector3();
				if (nplayer != null)
				{
					nplayer.NewPos(pos, rot);
				}
			}
		}
	}

	private void ReceiveMessages(NetBuffer buffer)
	{
		while (buffer.IsEndOfRead() == false)
		{
			NetField field = (NetField)buffer.ReadByte();
			if (field == NetField.JUMP)
			{
				Debug.Log("xx-- receve jump message");

				if (nplayer != null)
				{
					nplayer.animator.SetBool("Jump", true);
				}
			}
			else if (field == NetField.ANI)
			{
				float speed = buffer.ReadSingle();
				float direction = buffer.ReadSingle();

				var comp = nplayer.GetComponent<IdleRunJump>();
				comp.nSpeed = speed;
				comp.nDirection = direction;
			}
		}
	}

	private void ParsePacket(byte chan, ENet.Packet pkt)
	{
		NetBuffer buffer = new NetBuffer();
		buffer.data = pkt.GetBytes();

		ChanId chanId = (ChanId)chan;
		if (chanId == ChanId.POS)
			ReceivePositions(buffer);
		else if (chanId == ChanId.MESSAGE)
			ReceiveMessages(buffer);
	}

	public void AddMessage(NetField field, params object[] vals)
	{
		if (messages == null)
			messages = new NetBuffer();

		messages.Write((byte)field);

		for (int i = 0; i < vals.Length; ++i)
		{
			object obj = vals[i];

			Type t = obj.GetType();
			if (t == typeof(int))
				messages.Write((int)obj);
			else if (t == typeof(float))
				messages.Write((float)obj);
			else if (t == typeof(Vector3))
				messages.Write((Vector3)obj);
			else
				Debug.Assert(false, "CHECK: cant handle type > " + t.ToString());
		}

		// messages.Add(buffer);
		Debug.Log("xx-- add message");
	}
}
