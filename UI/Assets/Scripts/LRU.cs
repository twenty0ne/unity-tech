using System;
using System.Collections.Generic;

public class LRU<TKey, TValue>
{
	private const int DEFAULT_CAPACITY = 255;

	private int _capacity = 0;
	private Dictionary<TKey, TValue> hashMap;
	private LinkedList<TKey> linkedList;

	public LRU() 
	{
		_capacity = DEFAULT_CAPACITY;
	}

	public LRU(int capacity)
	{
		_capacity = capacity;
	}

	public void Add(TKey key)
	{
		// if (hashMap.Count >= )
	}

	private void Remove()
	{

	}
}
