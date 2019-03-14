using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pathfinding.Serialization.JsonFx;

namespace ZData
{
    public enum EShellType
    {
        armorPiercing,
        highExplosive
    }

    public class ShellInfoItem
    {
        public string id;
        public EShellType type;
        public int damage;
        public int armorPiercing;
        public int costMoney;
        public int costPremium;
    }

    public class ShellInfo
    {
        public List<ShellInfoItem> items = new List<ShellInfoItem>();

        [JsonIgnore]
        public Dictionary<string, ShellInfoItem> dicItems = new Dictionary<string, ShellInfoItem>();

        public void Init()
        {
            foreach (ShellInfoItem it in items)
            {
                dicItems.Add(it.id, it);
            }
        }

        public ShellInfoItem this[string id]
        {
            get { return dicItems[id]; }
        }

    }
}
