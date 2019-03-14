using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

namespace ZData
{
    public class UnitInfoItem
    {
        public string id;
        public int cost;
        public int ap;
        public int apMove;
        public int apRotate;
        public int apReload;
        public int apShot;
        public int health;
        public int[] armor;
        public List<string> shells;
        public int costRepair;
        public int costRepairArmor;
    }

    public class UnitInfo
    {
        public List<UnitInfoItem> items = new List<UnitInfoItem>();

        [JsonIgnore]
        public Dictionary<string, UnitInfoItem> dicItems = new Dictionary<string, UnitInfoItem>();


        public void Init()
        {
            foreach (UnitInfoItem it in items)
            {
                dicItems.Add(it.id, it);
            }
        }

        public UnitInfoItem this[string id]
        {
            get { return dicItems[id]; }
        }
    }
}
