using System.Collections.Generic;

namespace ZData
{
	public class EventChannel : IEventChannel
	{
		public Dictionary<string, List<ISubscriber>> subscribers = new Dictionary<string, List<ISubscriber>>();


		public void Publish(string type, PublisherData data)
		{
			if (!subscribers.ContainsKey(type))
				return;
			foreach (ISubscriber s in subscribers[type])
				s.Notify(data);
		}

		public void Subscribe(ISubscriber subscriber, string type)
		{
			if (subscribers.ContainsKey(type))
				subscribers[type].Add(subscriber);
			else
				subscribers.Add(type, new List<ISubscriber> {subscriber});
		}

		public void UnSubscribe(ISubscriber subscriber, string type)
		{
			if (!subscribers.ContainsKey(type))
				return;
			subscribers[type].Remove(subscriber);
		}
	}
}
