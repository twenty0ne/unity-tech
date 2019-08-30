using System.Collections.Generic;

// 非线程安全
public class Pool<T> where T : new()
{
	private int _defaultSize = 0;
	private List<T> _allGroup = new List<T>();
	private List<T> _freeGroup = new List<T>();

	public Pool(int defaultSize)
	{
		_defaultSize = defaultSize;

		for (int i = 0; i < defaultSize; ++i)
		{
			T obj = new T();
			_freeGroup.Add(obj);
			_allGroup.Add(obj);
		}
	}

	public T Spawn()
	{
		if (_freeGroup.Count == 0)
		{
			T nobj = new T();
			_allGroup.Add(nobj);
			return nobj;
		}

		T obj = _freeGroup[0];
		_freeGroup.RemoveAt(0);
		return obj;
	}

	public void Recycle(T obj)
	{
		_freeGroup.Add(obj);
	}

	// public int Count
	// {
	// 	get { return _allGroup.Count; }
	// }
	public void ClearAll()
	{
		for (int i = 0; i < _freeGroup.Count; ++i)
			_freeGroup[i] = default(T);
		_freeGroup.Clear();

		for (int i = 0; i < _allGroup.Count; ++i)
			_allGroup[i] = default(T);
		_allGroup.Clear();
	}
} 