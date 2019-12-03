using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventListener
{

}

public class EventManager : Singleton<EventManager>
{
	public delegate void EventCallback(Event evt);

	public Dictionary<string, Dictionary<IEventListener, EventCallback>> listeners = new Dictionary<string, Dictionary<IEventListener, EventCallback>>();

	public void Add(string evtName, IEventListener lt, EventCallback cb)
	{
		Debug.Assert(string.IsNullOrEmpty(evtName) == false, "CHECK");
		Debug.Assert(lt != null, "CHECK");
		Debug.Assert(cb != null, "CHECK");

		if (!listeners.ContainsKey(evtName))
			listeners.Add(evtName, new Dictionary<IEventListener, EventCallback>());

		listeners[evtName].Add(lt, cb);
	}

	public void Remove(string evtName, IEventListener lt)
	{
		Debug.Assert(string.IsNullOrEmpty(evtName) == false, "CHECK");
		Debug.Assert(lt != null, "CHECK");

		if (listeners.ContainsKey(evtName))
		{
			listeners[evtName].Remove(lt);
		}
		else
		{
			Debug.LogWarning("failed to remove " + evtName + ", because cant find.");
		}
	}

	public void RemoveAll(IEventListener lt)
	{
		Debug.Assert(lt != null, "CHECK");
		
		foreach (var kv in listeners)
		{
			if (kv.Value.ContainsKey(lt))
			{
				kv.Value.Remove(lt);
			}
		}
	}

	public void Dispatch(string name)
	{
		Event evt = new Event();
		evt.name = name;
		Dispatch(evt);
	}

	public void Dispatch(string name, int val)
	{
		EventSimpleInt evt = new EventSimpleInt();
		evt.name = name;
		evt.val = val;
		Dispatch(evt);
	}

	public void Dispatch(string name, float val)
	{
		EventSimpleFloat evt = new EventSimpleFloat();
		evt.name = name;
		evt.val = val;
		Dispatch(evt);
	}

	public void Dispatch(string name, string val)
	{
		EventSimpleString evt = new EventSimpleString();
		evt.name = name;
		evt.val = val;
		Dispatch(evt);
	}

	public void Dispatch(Event evt)
	{
		Debug.Assert(evt != null, "CHECK");

		if (listeners.ContainsKey(evt.name) == false)
		{
			Debug.LogWarning("failed to Dispatch event " + evt.name + ", because no listener.");
			return;
		}

		foreach (var kv in listeners[evt.name])
		{
			kv.Value.Invoke(evt);
		}
	}
}