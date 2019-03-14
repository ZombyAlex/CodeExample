using System;
using System.Collections.Generic;
using System.IO;
using Pathfinding.Serialization.JsonFx;
using ZData;

namespace ZServer.Game
{
   
    public class ZGameData
    {
        public Random rnd = new Random();

        private Dictionary<uint, User> users = new Dictionary<uint, User>();
        private Dictionary<uint, PrepareBattle> prepareBattles = new Dictionary<uint, PrepareBattle>();
        private Dictionary<uint, Battle> battles = new Dictionary<uint, Battle>();

        private ZConfig config;


        public void Load()
        {
            StreamReader reader = new StreamReader("data/config.json");
            string text = reader.ReadToEnd();
            reader.Close();
            config = JsonReader.Deserialize<ZConfig>(text);
        }

        public void Save()
        {
            StreamWriter writer = new StreamWriter("data/config.json", false);
            string text = JsonWriter.Serialize(config);
            writer.Write(text);
            writer.Close();
        }

        public void AddUser(User user)
        {
            if (!users.ContainsKey(user.id))
            {
                users.Add(user.id, user);
                foreach (Unit unit in user.units)
                {
                    if (unit.uid == 0)
                        unit.uid = GetUnitUID();
                }
            }
        }

        public User GetUser(uint userId)
        {
            if (users.ContainsKey(userId))
                return users[userId];
            Tools.Log("logs/error_game.txt", "no find user: id=" + userId);
            return null;
        }

        public bool IsUser(uint userId)
        {
            return users.ContainsKey(userId);
        }

        public User GetUserDeviceId(string deviceId)
        {
            foreach (var user in users)
            {
                if(user.Value.deviceId == deviceId)
                    return user.Value;
            }
            return null;
        }

        public void UpdateDisconnect(float dt)
        {
            List<uint> delUsers = new List<uint>();
            foreach (KeyValuePair<uint, User> user in users)
            {
                if (!user.Value.isConnection && !user.Value.isBattle)
                {
                    user.Value.timeDisconnect += dt;
                    if (user.Value.timeDisconnect >= GameConst.timeDisconnectUnload)
                    {
                        delUsers.Add(user.Key);
                        UnlodUser(user.Value);
                    }
                }
                else
                    user.Value.timeDisconnect = 0;
            }

            foreach (uint u in delUsers)
            {
                users.Remove(u);
            }
        }

        private void UnlodUser(User user)
        {
            Console.WriteLine("Unload user=" + user.id);
            Data.t.gameToDataBase.Add(new MsgGameSaveUser(user.id, user));
        }

        private uint GetPrepareBattleId()
        {
            uint id = 0;

            do
            {
                id++;
            } while (prepareBattles.ContainsKey(id));
            return id;
        }

        public uint CreatePrepareBattle(User user, EBattleMode mode)
        {
            uint id = GetPrepareBattleId();
            PrepareBattle battle = new PrepareBattle() {id = id, time = 10, mode = mode};
            battle.users.Add(new PrepareBattleItem(user.id, user.name));
            prepareBattles.Add(battle.id, battle);
            return id;
        }

        public void UpdatePrepareBattles(float dt)
        {
            List<uint> delList = new List<uint>();

            foreach (KeyValuePair<uint, PrepareBattle> it in prepareBattles)
            {
                it.Value.time -= dt;
                for (int i = 0; i < it.Value.users.Count; i++)
                {
                    User user = GetUser(it.Value.users[i].userId);
                    bool isDel = user == null;
                    if (!isDel)
                    {
                        if (!user.isConnection)
                            isDel = true;
                    }

                    if (isDel)
                    {
                        it.Value.users.RemoveAt(i);
                        i--;
                    }
                }

                if (it.Value.users.Count == 0)
                {
                    delList.Add(it.Key);
                }
                else if (it.Value.time <= 0 && it.Value.users.Count > 0)
                {
                    StartBattle(it.Value);
                    delList.Add(it.Key);
                }
            }


            for (int i = 0; i < delList.Count; i++)
            {
                prepareBattles.Remove(delList[i]);
            }
        }

        private void StartBattle(PrepareBattle prepareBattle)
        {
            uint id = GetBattleId();
            Battle battle = new Battle();
            battle.Create(prepareBattle, rnd.Next(0, Int32.MaxValue));
            battles.Add(id, battle);

            BattleData data = battle.GetData();
            for (int i = 0; i < data.users.Count; i++)
            {
                Data.t.netMsg.Add(new MsgServerCreateBattle(data.users[i].userid, battle.GetSeed(), battle.GetSize()));
                Data.t.netMsg.Add(new MsgServerBattleData(data.users[i].userid, data));
                User user = GetUser(data.users[i].userid);
                if (user != null)
                {
                    user.isBattle = true;
                    user.battleId = id;
                    Unit unit = user.GetSelectUnit();
                    unit.RestoreUnitBattle();
                    unit.pos = battle.GetFreeStartPos(rnd);
                    battle.GetMap().SetUnitPos(unit.pos);
                    unit.dir = rnd.Next(0, 4);
                    Data.t.netMsg.Add(new MsgServerGameAction(user.id, new GameActionVisible(unit, true)));
                }
            }
        }

        public void AddUserPrepareBattle(User user, EBattleMode mode)
        {
            PrepareBattle prepareBattle = GetPrepareBattle(mode);
            if (prepareBattle == null)
            {
                uint id = CreatePrepareBattle(user, mode);
                Data.t.netMsg.Add(new MsgServerPrepareBattle(user.id, prepareBattles[id]));
                return;
            }
            Data.t.netMsg.Add(new MsgServerPrepareBattle(user.id, prepareBattle));
        }

        private PrepareBattle GetPrepareBattle(EBattleMode mode)
        {
            for (int i = 2; i < 5; i++)
            {
                foreach (KeyValuePair<uint, PrepareBattle> it in prepareBattles)
                {
                    if (it.Value.mode == mode && it.Value.users.Count < i)
                    {
                        return it.Value;
                    }
                }
            }
            return null;
        }

        public void UserExitPrepareBattle(uint userId)
        {
            foreach (KeyValuePair<uint, PrepareBattle> it in prepareBattles)
            {
                for (int i = 0; i < it.Value.users.Count; i++)
                {
                    if (it.Value.users[i].userId == userId)
                    {
                        it.Value.users.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private uint GetBattleId()
        {
            uint id = 0;

            do
            {
                id++;
            } while (battles.ContainsKey(id));
            return id;
        }

        public uint GetUnitUID()
        {
            config.unitUID++;
            return config.unitUID;
        }

        public Battle GetBattle(uint battleId)
        {
            if (battles.ContainsKey(battleId))
                return battles[battleId];
            return null;
        }

        public Vector2w GetBattleUnitPos(uint userId)
        {
            User user = GetUser(userId);
            return user.GetSelectUnit().pos;
        }

        public Unit GetBattleUnit(uint userId)
        {
            User user = GetUser(userId);
            return user.GetSelectUnit();
        }

        public void UpdateBattles(float dt)
        {
            foreach (KeyValuePair<uint, Battle> battle in battles)
            {
                BattleData battleData = battle.Value.GetData();
                battleData.timeTurn -= dt;

            }
        }
    }
}
