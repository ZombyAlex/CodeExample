using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pathfinding.Serialization.JsonFx;

namespace ZData
{
    public class BlockInfoItemSprite
    {
        public int health;
        public string sprite;
    }

    public class BlockInfoItem
    {
        public string id;
        public bool isMove;
        public bool isVisible;
        public int health;

        public List<BlockInfoItemSprite> sprites;


        public string GetSprites(int health)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].health < health)
                    return sprites[i].sprite;
            }
            return string.Empty;
        }
    }

    public class BlockInfo
    {
        public List<BlockInfoItem> items = new List<BlockInfoItem>();

        [JsonIgnore]
        public Dictionary<string, BlockInfoItem> dicItems = new Dictionary<string, BlockInfoItem>();

        public void Init()
        {
            foreach (BlockInfoItem it in items)
            {
                dicItems.Add(it.id, it);
            }
        }

        public BlockInfoItem this[string id]
        {
            get { return dicItems[id]; }
        }
    }
}
