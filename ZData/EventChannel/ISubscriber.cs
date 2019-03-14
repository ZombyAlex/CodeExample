
namespace ZData
{
	public interface ISubscriber
	{
		void Notify(PublisherData data);
		void Remove();
	}
}
