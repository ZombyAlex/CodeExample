using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZData;

namespace ZServer
{
	public class SubscriberTest : Subscriber
	{
		public SubscriberTest(string type, IEventChannel eventChannel)
			: base(type, eventChannel)
		{
		}

		public override void Notify(PublisherData data)
		{
			PublisherDataTest dt = (PublisherDataTest)data;
			Console.WriteLine(dt.Text()+ "; type="+dt.Type());
		}
	}

	public class PublisherDataTest: PublisherData
	{
		private string text;

		public PublisherDataTest(string type, string text)
			: base(type)
		{
			this.text = text;
		}

		public string Text()
		{
			return text;
		}
	}

	public class EventChannelTest
	{
		public void Test()
		{
			EventChannel eventChannel = new EventChannel();

			SubscriberTest subscriber = new SubscriberTest("group1", eventChannel);
			SubscriberTest subscriber2 = new SubscriberTest("group2", eventChannel);


			eventChannel.Publish("group1", new PublisherDataTest("evnType1", "test 1"));
			eventChannel.Publish("group1", new PublisherDataTest("evnType2", "data text"));

			eventChannel.Publish("group2", new PublisherDataTest("evnType1", "test 111"));

			subscriber.Remove();

			eventChannel.Publish("group1", new PublisherDataTest("evnType1", "test 2"));
			eventChannel.Publish("group2", new PublisherDataTest("evnType1", "test 3"));

		}
	}
}
