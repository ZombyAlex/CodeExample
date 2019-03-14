using Lidgren.Network;

namespace ZData
{
	public class MsgGame
	{
        private EMsgGame t;
		private uint id;
		//private NetConnection address;


	    public MsgGame(EMsgGame t, uint id)
	    {
	        this.t = t;
	        this.id = id;
	    }

	    public EMsgGame T
	    {
	        get { return t; }
	        set { t = value; }
	    }

	    public uint Id
	    {
	        get { return id; }
	        set { id = value; }
	    }
        /*
	    public NetConnection Address
	    {
	        get { return address; }
	        set { address = value; }
	    }
         */
	}

    public class MsgGameUserStatusConnect : MsgGame
    {
        public bool statusConnect;

        public MsgGameUserStatusConnect(uint id, bool statusConnect) : base(EMsgGame.userStatusConnect, id)
        {
            this.statusConnect = statusConnect;
        }
    }

    public class MsgGameLoginUserDeviceId : MsgGame
    {
        public string deviceId;
        public NetConnection address;

        public MsgGameLoginUserDeviceId(uint id, string deviceId, NetConnection address)
            : base(EMsgGame.loginUserDeviceId, id)
        {
            this.deviceId = deviceId;
            this.address = address;
        }
    }

    public class MsgGameLoadUser : MsgGame
    {
        public User user;
        public NetConnection address;

        public MsgGameLoadUser(uint id, User user, NetConnection address)
            : base(EMsgGame.loadUser, id)
        {
            this.user = user;
            this.address = address;
        }
    }

    public class MsgGameUserData : MsgGame
    {
        public User user;
        public NetConnection address;

        public MsgGameUserData(uint id, User user, NetConnection address)
            : base(EMsgGame.userData, id)
        {
            this.user = user;
            this.address = address;
        }
    }

    public class MsgGameSaveUser : MsgGame
    {
        public User user;

        public MsgGameSaveUser(uint id, User user)
            : base(EMsgGame.saveUser, id)
        {
            this.user = user;
        }
    }
}
