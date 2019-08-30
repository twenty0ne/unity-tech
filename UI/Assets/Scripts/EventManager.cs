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

	public void Add(string eventName, IEventListener lt, EventCallback cb)
	{
		Debug.Assert(string.IsNullOrEmpty(eventName) == false, "CHECK");
		Debug.Assert(lt != null, "CHECK");
		Debug.Assert(cb != null, "CHECK");

		if (!listeners.ContainsKey(eventName))
			listeners.Add(eventName, new Dictionary<IEventListener, EventCallback>());

		listeners[eventName].Add(lt, cb);
	}

	public void Remove(string eventName, IEventListener lt)
	{
		Debug.Assert(string.IsNullOrEmpty(eventName) == false, "CHECK");
		Debug.Assert(lt != null, "CHECK");

		if (listeners.ContainsKey(eventName))
		{
			listeners[eventName].Remove(lt);
		}
		else
		{
			Debug.LogWarning("failed to remove " + eventName + ", because cant find.");
		}
	}

	public void Remove(IEventListener lt)
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

	public void Dispatch(string eventName)
	{
		Event evt = new Event();
		evt.name = eventName;
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