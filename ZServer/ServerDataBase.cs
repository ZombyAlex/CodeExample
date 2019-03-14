using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZData;
using ZServer.Game;

namespace ZServer
{
    public class ServerDataBase: GameThread
    {
        private ZDataBase dataBase = new ZDataBase();

        public ServerDataBase(int sleepWait, string threadName) : base(sleepWait, threadName)
        {
        }


        protected override void Init()
        {
            AddSheduleCall(0.5f, UpdateInNetMsg, 0.0f);
            AddSheduleCall(0.1f, UpdateInGameMsg, 0.1f);
            AddSheduleCall(600, dataBase.Save, 0.2f);
        }

        protected override void OnTerminate()
        {
            dataBase.Save();
        }

        protected override void Update(float dt)
        {
            
        }

        private void UpdateInNetMsg()
        {
            Data.t.netToDataBase.ToWork();
            if (Data.t.netToDataBase.IsWork())
            {
                var list = Data.t.netToDataBase.GetWork();
                for (int i = 0; i < list.Count; i++)
                {
                    MsgNetClient msg = (MsgNetClient) list[i];
                    dataBase.ApplyMsgNet(msg);
                }
            }
        }

        private void UpdateInGameMsg()
        {
            Data.t.gameToDataBase.ToWork();
            if (Data.t.gameToDataBase.IsWork())
            {
                var list = Data.t.gameToDataBase.GetWork();
                for (int i = 0; i < list.Count; i++)
                {
                    MsgGame msg = (MsgGame)list[i];
                    dataBase.ApplyMsgGame(msg);
                }
            }
        }
    }
}
