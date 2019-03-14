using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pathfinding.Serialization.JsonFx;

namespace ZData
{
    public class MsgServerUser : MsgNetServer
    {
        public User user;

        public MsgServerUser()
        {
        }

        public MsgServerUser(uint userId, User user)
            : base(EMsgNetServer.user)
        {
            id = userId;
            this.user = (User)user.Clone();
            json = JsonWriter.Serialize(this);
        }
    }

    public class MsgServerSelectUnit : MsgNetServer
    {
        public string unitId;

        public MsgServerSelectUnit()
        {
        }

        public MsgServerSelectUnit(uint userId, string unitId)
            : base(EMsgNetServer.selectUnit)
        {
            id = userId;
            this.unitId = unitId;
            json = JsonWriter.Serialize(this);
        }
    }

    public class MsgServerNotification : MsgNetServer
    {
        public string notification;

        public MsgServerNotification()
        {
        }

        public MsgServerNotification(uint userId, string notification)
            : base(EMsgNetServer.notification)
        {
            id = userId;
            this.notification = notification;
            json = JsonWriter.Serialize(this);
        }
    }

    public class MsgServerPrepareBattle : MsgNetServer
    {
        public PrepareBattle battle;

        public MsgServerPrepareBattle()
        {
        }

        public MsgServerPrepareBattle(uint userId, PrepareBattle battle)
            : base(EMsgNetServer.prepareBattle)
        {
            id = userId;
            this.battle = battle;
            json = JsonWriter.Serialize(this);
        }
    }

    public class MsgServerCreateBattle : MsgNetServer
    {
        public int seed;
        public Vector2w size;

        public MsgServerCreateBattle()
        {
        }

        public MsgServerCreateBattle(uint userId, int seed, Vector2w size)
            : base(EMsgNetServer.createBattle)
        {
            id = userId;
            this.seed = seed;
            this.size = size;
            json = JsonWriter.Serialize(this);
        }
    }

    public class MsgServerBattleData : MsgNetServer
    {
        public BattleData data;

        public MsgServerBattleData()
        {
        }

        public MsgServerBattleData(uint userId, BattleData data)
            : base(EMsgNetServer.battleData)
        {
            id = userId;
            this.data = data;
            json = JsonWriter.Serialize(this);
        }
    }

    public class MsgServerGameAction : MsgNetServer
    {
        public GameAction action;

        public MsgServerGameAction()
        {
        }

        public MsgServerGameAction(uint userId, GameAction action)
            : base(EMsgNetServer.gameAction)
        {
            id = userId;
            this.action = action;
            action.Write();
            json = JsonWriter.Serialize(this);
        }
    }

    public class MsgServerGameActionList : MsgNetServer
    {
        public List<GameAction> actions;

        public MsgServerGameActionList()
        {
        }

        public MsgServerGameActionList(uint userId, List<GameAction> actions)
            : base(EMsgNetServer.gameActionList)
        {
            id = userId;
            this.actions = actions;
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].Write();
            }
            json = JsonWriter.Serialize(this);
        }
    }
}
