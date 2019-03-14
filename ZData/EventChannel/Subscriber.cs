
namespace ZData
{
	public class Subscriber : ISubscriber
	{
		private readonly string type;
		private readonly IEventChannel eventChannel;

		public Subscriber(string type, IEventChannel eventChannel)
		{
			this.type = type;
			this.eventChannel = eventChannel;
			eventChannel.Subscribe(this, type);
		}

		public virtual void Notify(PublisherData data)
		{
			
		}

		public void Remove()
		{
			eventChannel.UnSubscribe(this, type);
		}
	}
}
