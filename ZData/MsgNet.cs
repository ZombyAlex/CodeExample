using Lidgren.Network;
using Pathfinding.Serialization.JsonFx;

namespace ZData
{
	public class MsgNet
	{
        //[JsonIgnore]
		//public EMsgNet type;
        [JsonIgnore]
		public NetConnection address;
        [JsonIgnore]
		public uint id;
        [JsonIgnore]
		public string json;
	}

    public class MsgNetServer : MsgNet
    {
        [JsonIgnore]
        public EMsgNetServer type;

        public MsgNetServer()
        {
        }

        public MsgNetServer(EMsgNetServer type)
        {
            this.type = type;
        }

        public MsgNetServer(EMsgNetServer type, string json)
		{
			this.type = type;
			this.json = json;
		}

		public void Read(NetIncomingMessage inMsg)
		{
            type = (EMsgNetServer)inMsg.ReadUInt16();
			json = GameUtil.ReadString(inMsg);
		}

		public void Write(NetOutgoingMessage outMsg)
		{
			outMsg.Write((ushort)type);
			GameUtil.WriteString(outMsg, json);
		}

    }

    public class MsgNetClient : MsgNet
    {
        [JsonIgnore]
        public EMsgNetClient type;

        public MsgNetClient()
        {
        }

        public MsgNetClient(EMsgNetClient type)
        {
            this.type = type;
        }

        public MsgNetClient(EMsgNetClient type, string json)
        {
            this.type = type;
            this.json = json;
        }

        public void Read(NetIncomingMessage inMsg)
        {
            type = (EMsgNetClient)inMsg.ReadUInt16();
            json = GameUtil.ReadString(inMsg);
        }

        public void Write(NetOutgoingMessage outMsg)
        {
            outMsg.Write((ushort)type);
            GameUtil.WriteString(outMsg, json);
        }

    }
}
