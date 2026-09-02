using System;

namespace GameLogic.Scripts.EventBus
{
	public interface IEventBus
	{
		public void Subscribe<TEvent>(Action<TEvent> action);

		public void UnsubscribeAll();
		void Unsubscribe<TEvent>(Action<TEvent> action);
		void Publish<TEvent>(TEvent eventData);
		void Subscribe(Type type, Action<object> onEvent);
		void Unsubscribe(Type type, Action<object> onEvent);
	}
}