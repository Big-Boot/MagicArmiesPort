using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic.Scripts.EventBus
{
	public class GameEventBus : IEventBus
	{
		private readonly Dictionary<Type, List<Delegate>> events = new();
		public void Subscribe(Type type, Action<object> action)
		{
			if (!events.ContainsKey(type)) events.Add(type, new List<Delegate>());
			events[type].Add(action);
		}
		public void Subscribe<TEvent>(Action<TEvent> action)
		{
			var eventType = typeof(TEvent);
			if (!events.ContainsKey(eventType)) events.Add(eventType, new List<Delegate>());
			events[eventType].Add(action);
		}

		public void UnsubscribeAll()
        {
			events.Clear();
        }
		public void Unsubscribe<TEvent>(Action<TEvent> action)
		{
			var eventType = typeof(TEvent);
			if (events.ContainsKey(eventType))
			{
				var actions = events[eventType];
				events[eventType].Remove(action);
			}
		}
		public void Unsubscribe(Type type, Action<object> action)
		{
			if (events.ContainsKey(type))
			{
				var actions = events[type];
				events[type].Remove(action);
			}
		}

		public void Publish<TEvent>(TEvent eventData)
		{
			var eventType = typeof(TEvent);
			if (events.ContainsKey(eventType))
				foreach (Action<TEvent> action in events[eventType].ToList())
					action?.Invoke(eventData);
		}
	}
}