// 参考 lidgren NetBuffer 实现

using System;
using System.Diagnostics;

public class NetBuffer
{
	/// <summary>
	/// Number of bytes to overallocate for each message to avoid resizing
	/// </summary>
	protected const int c_overAllocateAmount = 0;

	private byte[] m_data;
	private int m_byteLength = 0;
	private int m_readPosition = 0;	// byte

	private static void WriteByte(byte src, byte[] dst, int dstOffset)
	{
		dst[dstOffset] = src;
	}

	private static void WriteBytes(byte[] src, int srcOffset, int byteLen, byte[] dst, int dstOffset)
	{
		Buffer.BlockCopy(src, srcOffset, dst, dstOffset, byteLen);
	}

	public byte[] data { get { return m_data; } set { m_data = value;  } }
	public int length { get { return m_data.Length;  } }

	public void Write(byte value)
	{
		EnsureBufferSize(m_byteLength + 1);
		WriteByte(value, m_data, m_byteLength);
		m_byteLength += 1;
	}

	public void Write(Int32 value)
	{
		EnsureBufferSize(m_byteLength + 4);
		byte[] bytes = BitConverter.GetBytes(value);
		WriteBytes(bytes, 0, 4, m_data, m_byteLength);
		m_byteLength += 4;
	}
	
	public void Write(float value)
	{
		EnsureBufferSize(m_byteLength + 4);
		byte[] bytes = BitConverter.GetBytes(value);
		WriteBytes(bytes, 0, 4, m_data, m_byteLength);
		m_byteLength += 4;
	}

	//public void Write(UnityEngine.Vector2 value)
	//{
	//	EnsureBufferSize(m_byteLength + 8);
	//	Write(value.x);
	//	Write(value.y);
	//}

	public void Write(UnityEngine.Vector3 value)
	{
		EnsureBufferSize(m_byteLength + 12);
		Write(value.x);
		Write(value.y);
		Write(value.z);
	}

	public byte ReadByte()
	{
		Debug.Assert(m_readPosition + 1 > length, "CHECK");
		byte val = m_data[m_readPosition];
		m_readPosition += 1;
		return val;
	}

	public Int32 ReadInt32()
	{
		Debug.Assert(m_readPosition + 4 > length, "CHECK");
		Int32 val = BitConverter.ToInt32(m_data, m_readPosition);
		m_readPosition += 4;
		return val;
	}

	public float ReadSingle()
	{
		Debug.Assert(m_readPosition + 4 > length, "CHECK");
		float val = BitConverter.ToSingle(m_data, m_readPosition);
		m_readPosition += 4;
		return val;
	}

	public UnityEngine.Vector3 ReadVector3()
	{
		Debug.Assert(m_readPosition + 12 > length, "CHECK");
		float x = ReadSingle();
		float y = ReadSingle();
		float z = ReadSingle();
		return new UnityEngine.Vector3(x, y, z);
	}

	public bool IsEndOfRead()
	{
		return m_readPosition >= m_data.Length;
	}

	private void EnsureBufferSize(int byteLen)
	{
		if (m_data == null)
		{
			m_data = new byte[byteLen + c_overAllocateAmount];
			return;
		}
		if (m_data.Length < byteLen)
			Array.Resize<byte>(ref m_data, byteLen + c_overAllocateAmount);
	}
}
