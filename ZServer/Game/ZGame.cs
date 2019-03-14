using System;
using Lidgren.Network;
using Pathfinding.Serialization.JsonFx;
using ZData;

namespace ZServer.Game
{
    public class ZGame
    {
        private ZGameData d = new ZGameData();
        private ZBattleManager battleManager;

        public ZGame()
        {
            battleManager = new ZBattleManager(d);
            d.Load();
        }

        public void ApplyMsgNet(MsgNetClient msg)
        {
            switch (msg.type)
            {
                case EMsgNetClient.loginIdDevice:
                {
                    MsgClientLoginIdDevice m = JsonReader.Deserialize<MsgClientLoginIdDevice>(msg.json);
                    LoginIdDevice(msg.id, m.deviceId, msg.address);
                }
                    break;
                case EMsgNetClient.selectUnit:
                {
                    MsgClientSelectUnit m = JsonReader.Deserialize<MsgClientSelectUnit>(msg.json);
                    SelectUnit(msg.id, m.unitId);
                }
                    break;

                case EMsgNetClient.buyUnit:
                {
                    MsgClientBuyUnit m = JsonReader.Deserialize<MsgClientBuyUnit>(msg.json);
                    BuyUnit(msg.id, m.unitId);
                }
                    break;
                case EMsgNetClient.startBattle:
                {
                    MsgClientStartBattle m = JsonReader.Deserialize<MsgClientStartBattle>(msg.json);
                    StartPrepareBattle(msg.id, m.mode);
                }
                    break;
                case EMsgNetClient.exitPrepareBattle:
                {
                    ExitPrepareBattle(msg.id);
                }
                    break;

                case EMsgNetClient.unitMove:
                {
                    MsgClientUnitMove m = JsonReader.Deserialize<MsgClientUnitMove>(msg.json);
                    UnitMove(msg.id, m.pos);
                }
                    break;
                case EMsgNetClient.unitRotate:
                {
                    MsgClientUnitRotate m = JsonReader.Deserialize<MsgClientUnitRotate>(msg.json);
                    UnitRotate(m.id, m.dir);
                }
                    break;
                case EMsgNetClient.unitShot:
                {
                    MsgClientUnitShot m = JsonReader.Deserialize<MsgClientUnitShot>(msg.json);
                    UnitShot(m.id, m.pos);
                }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartPrepareBattle(uint userId, EBattleMode mode)
        {
            User user = d.GetUser(userId);
            if (user == null)
                return;

            Unit unit = user.GetSelectUnit();
            if (unit == null)
            {
                Data.t.netMsg.Add(new MsgServerNotification(userId, "no_select_unit"));
                return;
            }
            //TODO проверка полного здоровья юнита и наличия зарядов

            d.AddUserPrepareBattle(user, mode);
        }

        private void BuyUnit(uint userId, string unitId)
        {
            User user = d.GetUser(userId);
            if (user == null)
                return;
            if (user.units.Find(f => f.id == unitId) != null)
                return;
            UnitInfoItem info = GameData.unitInfo[unitId];
            int cost = info.cost;
            if (user.money < cost)
            {
                Data.t.netMsg.Add(new MsgServerNotification(userId, "no_money"));
                return;
            }
            user.money -= cost;
            user.units.Add(new Unit(unitId, d.GetUnitUID()));
            Data.t.netMsg.Add(new MsgServerUser(user.id, user));
        }

        private void SelectUnit(uint userId, string unitId)
        {
            Console.WriteLine("Select unit " + userId + " unit=" + unitId);
            User user = d.GetUser(userId);
            if (user == null)
                return;
            foreach (Unit unit in user.units)
            {
                unit.select = unit.id == unitId;
            }
            Data.t.netMsg.Add(new MsgServerSelectUnit(userId, unitId));
        }

        private void LoginIdDevice(uint userId, string deviceId, NetConnection address)
        {
            Console.WriteLine("LoginIdDevice id=" + userId + "; devId=" + deviceId);

            User user = d.GetUserDeviceId(deviceId);
            if (user == null)
                Data.t.gameToDataBase.Add(new MsgGameLoginUserDeviceId(userId, deviceId, address));
            else
            {
                UserStatusConnect(user.id, true);
                Data.t.gameToNet.Add(new MsgGameUserData(user.id, user, address));
                Data.t.netMsg.Add(new MsgServerUser(user.id, user));
            }
        }

        public void ApplyMsgGame(MsgGame msg)
        {
            switch (msg.T)
            {
                case EMsgGame.userStatusConnect:
                {
                    MsgGameUserStatusConnect m = (MsgGameUserStatusConnect) msg;
                    UserStatusConnect(m.Id, m.statusConnect);
                }
                    break;
                case EMsgGame.loadUser:
                {
                    MsgGameLoadUser m = (MsgGameLoadUser) msg;
                    m.user.isConnection = true;
                    Data.t.gameToNet.Add(new MsgGameUserData(m.user.id, m.user, m.address));
                    LoadUser(m.user);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UserStatusConnect(uint userId, bool statusConnect)
        {
            User user = d.GetUser(userId);
            if (user == null)
                return;
            Console.WriteLine("User status connection user=" + userId + "; status=" + statusConnect);
            user.isConnection = statusConnect;
        }

        private void LoadUser(User user)
        {
            Data.t.netMsg.Add(new MsgServerUser(user.id, user));
            d.AddUser(user);
        }

        public void UpdateDisconnect(float dt)
        {
           d.UpdateDisconnect(dt);
        }


        public void UpdatePrepareBattles(float dt)
        {
            d.UpdatePrepareBattles(dt);
        }

        private void ExitPrepareBattle(uint userId)
        {
            d.UserExitPrepareBattle(userId);
        }

        public void Save()
        {
            d.Save();
        }

        private void UnitMove(uint userId, Vector2w pos)
        {
            battleManager.UnitMove(userId, pos);
        }

        private void UnitRotate(uint userId, int dir)
        {
            battleManager.UnitRotate(userId, dir);
        }

        private void UnitShot(uint userId, Vector2w pos)
        {
            battleManager.UnitShot(userId, pos);
        }

        public void UpdateBattles(float dt)
        {
            battleManager.UpdateBattles(dt);
        }
    }
}
