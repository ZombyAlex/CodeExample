

using System;

namespace ZData
{
    public class Battle
    {
        private Map map;
        private BattleData data;
        private EBattleMode mode;
        private int seed;
        private Vector2w size;

        public FindRoad findRoad;

        public void Create(PrepareBattle prepareBattle, int seed)
        {
            mode = prepareBattle.mode;
            this.seed = seed;

            MapGenerator generator = new MapGenerator();
            int s = 24;
            if (prepareBattle.users.Count > 2)
                s = 32;
            size = new Vector2w(s, s);
            map = generator.Create(seed, size, GameData.blockInfo);

            data = new BattleData();
            data.Init(prepareBattle);

            findRoad = new FindRoad(size);
        }

        public BattleData GetData()
        {
            return data;
        }

        public int GetSeed()
        {
            return seed;
        }

        public Vector2w GetSize()
        {
            return size;
        }

        public Vector2w GetFreeStartPos(Random rnd)
        {
            int idx = rnd.Next(0, map.unitPoint.Count);
            Vector2w p = map.unitPoint[idx];
            map.unitPoint.RemoveAt(idx);
            return p;
        }

        public Map GetMap()
        {
            return map;
        }

        public bool IsUserTurn(uint userId)
        {
            BattleDataItem item = data.GetCurDataItem();
            return item.userid == userId;
        }
    }
}
