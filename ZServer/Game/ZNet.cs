using System;
using System.Collections.Generic;
using Lidgren.Network;
using ZData;

namespace ZServer.Game
{
    public class ZNet
    {
        private Dictionary<uint, UserNetData> userData = new Dictionary<uint, UserNetData>();
        private NetServer server;

        public ZNet(NetServer server)
        {
            this.server = server;
        }

        public void ProcMsg(NetIncomingMessage inMsg)
        {
            MsgNetClient msg = new MsgNetClient();
            msg.address = inMsg.SenderConnection;
            msg.Read(inMsg);
            UserNetData userNetData = GetUser(inMsg.SenderConnection);
            msg.id = userNetData == null ? 0 : userNetData.id;

            Data.t.netToGame.Add(msg);
        }

        private UserNetData GetUser(NetConnection inAddress)
        {
            foreach (KeyValuePair<uint, UserNetData> aUser in userData)
            {
                if (aUser.Value.address == inAddress)
                    return aUser.Value;
            }
            return null;
        }

        public void ChangeConnectStatusClient(NetConnection senderConnection, bool isConnected)
        {

            UserNetData user = GetUser(senderConnection);
            if (user != null)
            {
                user.connected = isConnected;
                Data.t.dataBaseToGame.Add(new MsgGameUserStatusConnect(user.id, isConnected));
                /*
                MsgUserStatusConnect msg = new MsgUserStatusConnect();
                msg.SetId(user.id);
                msg.SetType(EMsg.userStatusConnect);
                msg.statusConnect = isConnected;
                TransportNetGame.instance.msgs.Add(msg);
                 */
            }

        }

        public void UpdateOutMsg()
        {
            UpdateInMsgs();//должно вызыватся раньше чем сетевые сообщения

            //сетевые сообщения

            if (Data.t.netMsg.ToWorkFast())
            {
                var list = Data.t.netMsg.GetWork();
                for (int i = 0; i < list.Count; i++)
                {
                    MsgNetServer msg = (MsgNetServer)list[i];

                    if (userData.ContainsKey(msg.id))
                    {
                        Console.WriteLine("send msg = " + msg.type);
                        NetOutgoingMessage om = server.CreateMessage();
                        msg.Write(om);
                        server.SendMessage(om, userData[msg.id].address, NetDeliveryMethod.ReliableOrdered);
                    }
                }
            }
        }

        private void UpdateInMsgs()
        {
            if (Data.t.gameToNet.ToWorkFast())
            {
                var list = Data.t.gameToNet.GetWork();
                for (int i = 0; i < list.Count; i++)
                {
                    MsgGame msg = (MsgGame)list[i];
                    ApplyMsgGame(msg);
                }
            }
        }

        private void ApplyMsgGame(MsgGame msg)
        {
            switch (msg.T)
            {
                case EMsgGame.userData:
                {
                    MsgGameUserData m = (MsgGameUserData) msg;
                    if (!userData.ContainsKey(m.user.id))
                    {
                        UserNetData d = new UserNetData(string.Empty, string.Empty, m.user.id, string.Empty, m.address, 0, false);
                        userData.Add(m.user.id, d);
                    }
                    else
                    {
                        userData[m.user.id].address = m.address;
                    }
                }
                    break;
            }
        }
    }
}
