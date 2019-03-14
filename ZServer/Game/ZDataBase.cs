using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Pathfinding.Serialization.JsonFx;
using ZData;

namespace ZServer.Game
{
    public class ZDataBase
    {
        public ZDataBaseData data = new ZDataBaseData();

        public void Save()
        {
            data.Save();
        }

        public void ApplyMsgNet(MsgNetClient msg)
        {
            //TODO delete if not use
            switch (msg.type)
            {
                    
            }
        }

        public void ApplyMsgGame(MsgGame msg)
        {
            switch (msg.T)
            {
                case EMsgGame.loginUserDeviceId:
                {
                    MsgGameLoginUserDeviceId m = (MsgGameLoginUserDeviceId) msg;
                    UserLoginDeviceId(m.deviceId, m.address);
                }
                    break;
                case EMsgGame.saveUser:
                    {
                        MsgGameSaveUser m = (MsgGameSaveUser)msg;
                        data.SaveUser(m.user);
                    }
                    break;
            }
        }

        private void UserLoginDeviceId(string deviceId, NetConnection address)
        {
            if (data.IsUserDeviceId(deviceId))
            {
                LoadUserDeviceId(deviceId, address);
            }
            else
            {
                CreateUserDeviceId(deviceId, address);
            }
        }

        private void LoadUserDeviceId(string deviceId, NetConnection address)
        {
            User user = data.LoadUserDeviceId(deviceId);
            if (user != null)
                Data.t.dataBaseToGame.Add(new MsgGameLoadUser(user.id, user, address));
        }

        private void CreateUserDeviceId(string deviceId, NetConnection address)
        {
            User user = data.CreateUserDeviceId(deviceId, address);
            Data.t.dataBaseToGame.Add(new MsgGameLoadUser(user.id, user, address));
        }
    }
}
