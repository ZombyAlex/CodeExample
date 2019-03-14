
using System.Collections.Generic;

namespace ZData
{
    public class PrepareBattleItem
    {
        public uint userId;
        public string userName;

        public PrepareBattleItem()
        {
        }

        public PrepareBattleItem(uint userId, string userName)
        {
            this.userId = userId;
            this.userName = userName;
        }
    }

    public class PrepareBattle
    {
        public uint id;
        public List<PrepareBattleItem> users = new List<PrepareBattleItem>();
        public float time;
        public EBattleMode mode;
    }
}
