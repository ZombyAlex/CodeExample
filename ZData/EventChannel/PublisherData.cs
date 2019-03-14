
namespace ZData
{
	public class PublisherData
	{
		private readonly string type;

		protected PublisherData(string type)
		{
			this.type = type;
		}

		public string Type()
		{
			return type;
		}
	}
}
