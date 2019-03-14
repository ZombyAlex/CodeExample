using System.Collections.Generic;


namespace ZData
{
    public class BattleDataItem
    {
        public uint userid;
        public string userName;
    }

    public class BattleData
    {
        public List<BattleDataItem> users = new List<BattleDataItem>();
        public int curTurnIndex = 0;
        public float timeTurn = 0;

        public void Init(PrepareBattle prepareBattle)
        {
            foreach (PrepareBattleItem it in prepareBattle.users)
            {
                users.Add(new BattleDataItem(){userid = it.userId, userName = it.userName});
            }

            curTurnIndex = 0;
            timeTurn = GameConst.timeTurn;
        }

        public BattleDataItem GetCurDataItem()
        {
            return users[curTurnIndex];
        }
    }
}
