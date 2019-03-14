using ZData;
using ZServer.Game;


namespace ZServer
{
    public class ServerGame: GameThread
    {
        private ZGame game = new ZGame();

        public ServerGame(int sleepWait, string threadName) : base(sleepWait, threadName)
        {
        }

        protected override void Init()
        {
            AddSheduleCall(0.05f, UpdateInNetMsg, 0.0f);
            AddSheduleCall(0.05f, UpdateInGameMsg, 0.01f);
            AddSheduleUpdate(1.0f, game.UpdateDisconnect, 0.02f);
            AddSheduleUpdate(0.5f, game.UpdatePrepareBattles, 0.03f);
            AddSheduleUpdate(0.1f, game.UpdateBattles, 0.04f);
        }

        protected override void OnTerminate()
        {
            game.Save();
        }

        protected override void Update(float dt)
        {
            
        }

        private void UpdateInNetMsg()
        {
            if (Data.t.netToGame.ToWorkFast())
            {
                var list = Data.t.netToGame.GetWork();
                for (int i = 0; i < list.Count; i++)
                {
                    MsgNetClient msg = (MsgNetClient)list[i];
                    game.ApplyMsgNet(msg);
                }
            }
        }

        private void UpdateInGameMsg()
        {
            if (Data.t.dataBaseToGame.ToWorkFast())
            {
                var list = Data.t.dataBaseToGame.GetWork();
                for (int i = 0; i < list.Count; i++)
                {
                    MsgGame msg = (MsgGame) list[i];
                    game.ApplyMsgGame(msg);
                }
            }
        }
    }
}
