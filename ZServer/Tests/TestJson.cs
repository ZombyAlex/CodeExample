using System;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

namespace ZServer.Tests
{
	public class TestValueInt
	{
		private int val;

		public int Val
		{
			get { return val; }
			set { val = value; }
		}
	}

	public class TestValueFloat
	{
		private float val;

		public float Val
		{
			get { return val; }
			set { val = value; }
		}
	}

	public class TestData
	{
		private int info;
		public int Info
		{
			get { return info; }
			set
			{
				info = value; 
				UpdateValue();
			}
		}

		public Dictionary<string, int> items = new Dictionary<string, int>();

		public TestValueInt health = new TestValueInt();
		public TestValueInt damage = new TestValueInt();
		public TestValueFloat percent = new TestValueFloat();

		private void UpdateValue()
		{
			
		}
	}

	public class TestJson
	{
		public void Test()
		{
			TestData data = new TestData();
			data.Info = 55;
			data.items.Add("log", 10);
			data.items.Add("stone", 5);
			data.percent.Val = 0.49f;
			data.health.Val = 33;
			string json = JsonWriter.Serialize(data);

			Console.WriteLine(json);

			TestData d = JsonReader.Deserialize<TestData>(json);
			Console.WriteLine("info=" + d.Info);
			foreach (KeyValuePair<string, int> it in d.items)
			{
				Console.WriteLine("item=" + it.Key + " cnt=" + it.Value);
			}
		}
	}
}
