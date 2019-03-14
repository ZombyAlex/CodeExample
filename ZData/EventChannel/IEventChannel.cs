
namespace ZData
{
	public interface IEventChannel
	{
		void Publish(string type, PublisherData data);
		void Subscribe(ISubscriber subscriber, string type);
		void UnSubscribe(ISubscriber subscriber, string type);
	}
}
