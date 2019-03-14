using ZData;

namespace ZServer
{
	public class DataTransport
	{
		public VectorLock netToGame = new VectorLock();
		public VectorLock netToDataBase = new VectorLock();
		public VectorLock gameToNet = new VectorLock();//MsgGame
		public VectorLock gameToDataBase = new VectorLock();
		//public VectorLock dataBaseToNet = new VectorLock();
		public VectorLock dataBaseToGame = new VectorLock();

        public VectorLock netMsg = new VectorLock();//MsgNet
	}
}
