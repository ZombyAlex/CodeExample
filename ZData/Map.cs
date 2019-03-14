
using System.Collections.Generic;

namespace ZData
{
    public class Block
    {
        public string type;
        public int health;
    }

    public class MapItem
    {
        public int tile;
        public Block block;
    }

    public class Map
    {
        private Vector2w size;

        public Vector2w Size
        {
            get { return size; }
        }
        public MapItem[,] map;

        public List<Vector2w> unitPoint = new List<Vector2w>();

        private Dictionary<Vector2w, bool> unitPos = new Dictionary<Vector2w, bool>();

        public void Create(Vector2w size)
        {
            this.size = size;
            map = new MapItem[size.x, size.y];
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    map[x,y] = new MapItem();
                }
            }
        }

        public bool IsMap(Vector2w pos)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x >= size.x || pos.y >= size.y)
                return false;
            return true;
        }

        public bool IsBlock(Vector2w pos)
        {
            return map[pos.x, pos.y].block != null;
        }

        public void SetBlock(Vector2w pos, Block block)
        {
            map[pos.x, pos.y].block = block;
        }

        public bool IsMove(Vector2w pos, BlockInfo blockInfo)
        {
            if (IsUnitPos(pos))
                return false;
            if (map[pos.x, pos.y].block == null)
                return true;
            if(blockInfo[map[pos.x, pos.y].block.type].isMove)
                return true;
            return false;
        }

        public bool IsVisibleBlock(Vector2w pos, BlockInfo blockInfo)
        {
            if (map[pos.x, pos.y].block == null)
                return true;
            return blockInfo[map[pos.x, pos.y].block.type].isVisible;
        }

        public void RemoveUnitPos(Vector2w pos)
        {
            unitPos.Remove(pos);
        }

        public void SetUnitPos(Vector2w pos)
        {
            unitPos.Add(pos, true);
        }

        public bool IsUnitPos(Vector2w pos)
        {
            return unitPos.ContainsKey(pos);
        }

        public bool IsMoveBlock(Vector2w pos, BlockInfo blockInfo)
        {
            if (map[pos.x, pos.y].block == null)
                return true;
            return blockInfo[map[pos.x, pos.y].block.type].isMove;
        }
    }
}
