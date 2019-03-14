using System;
using System.Collections.Generic;

namespace ZData
{
    public class MapGenerator
    {
        private Random rnd;
        private Map map;

        private int[,] moveMap;

        private string[] blocks = new[] { "brick", "concrete_block" };
        private Vector2w[] offset = new[] {new Vector2w(-1, 0), new Vector2w(1, 0), new Vector2w(0, -1), new Vector2w(0, 1)};

        private BlockInfo blockInfo;

        public Map Create(int seed, Vector2w size, BlockInfo blockInfo)
        {
            this.blockInfo = blockInfo;
            moveMap = new int[size.x, size.y];
            rnd = new Random(seed);

            map = new Map();
            map.Create(size);

            GenerateTile();
            GenerateBlocks();
            GenerateUnitPoint();

            return map;
        }

        private void GenerateTile()
        {
            for (int x = 0; x < map.Size.x; x++)
            {
                for (int y = 0; y < map.Size.y; y++)
                {
                    map.map[x, y].tile = 0;
                }
            }
        }

        private void GenerateBlocks()
        {
            int percent = 20;
            int count = map.Size.x*map.Size.y*percent/100;

            for (int i = 0; i < count; i++)
            {
                Vector2w pos;
                do
                {
                    pos = new Vector2w(rnd.Next(0, map.Size.x), rnd.Next(0, map.Size.x));    
                } while (map.IsBlock(pos));

                int idxBlock = rnd.Next(0, blocks.Length);
                Block block = new Block() {health = blockInfo[blocks[idxBlock]].health, type = blocks[idxBlock]};
                map.SetBlock(pos, block);
            }
        }

        private void GenerateUnitPoint()
        {
            CreateMoveMap();
            List<Vector2w> list = new List<Vector2w>();
            //system find 4 point
            for (int x = 1; x < 4; x++)
            {
                for (int y = 1; y < 4; y++)
                {
                    if (x == 1 || x == 3)
                    {
                        if (y == 1 || y == 3)
                        {
                            list.Add(new Vector2w(map.Size.x/4*x, map.Size.y/4*x));
                        }
                    }
                }
            }

            OffsetUnitPoints(list);

            map.unitPoint = list;
        }

        private void OffsetUnitPoints(List<Vector2w> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (moveMap[list[i].x, list[i].y] == 0)
                {
                    list[i] = OffsetUnitPoint(list[i]);
                }
            }
        }

        private Vector2w OffsetUnitPoint(Vector2w pos)
        {
            int r = 1;
            do
            {
                for (int x = 0; x < r * 2 + 1; x++)
                {
                    for (int y = 0; y < r * 2 + 1; y++)
                    {
                        Vector2w p = new Vector2w(pos.x + x - 1, pos.y + y - 1);
                        if (map.IsMap(p) && moveMap[p.x, p.y] != 0)
                            return p;
                    }
                }
                r++;
            } while (true);
        }

        private void CreateMoveMap()
        {
            Dictionary<int, int> cnt = new Dictionary<int, int>();

            int idx = 1;
            bool res = false;
            while (true)
            {
                if (!SetStartPoint(idx))
                    break;
                cnt.Add(idx, 1);
                while (true)
                {
                    res = false;
                    for (int x = 0; x < map.Size.x; x++)
                    {
                        for (int y = 0; y < map.Size.y; y++)
                        {
                            if (moveMap[x, y] == idx)
                            {
                                res = FillAround(x, y, idx, cnt);
                            }
                        }
                    }
                    if (!res)
                        break;
                }
                idx++;
            }

            int max = 0;
            int memField = 0;
            foreach (KeyValuePair<int, int> it in cnt)
            {
                if (it.Value > max)
                {
                    max = it.Value;
                    memField = it.Key;
                }
            }

            for (int x = 0; x < map.Size.x; x++)
            {
                for (int y = 0; y < map.Size.y; y++)
                {
                    if (moveMap[x, y] == memField)
                        moveMap[x, y] = 1;
                    else
                        moveMap[x, y] = 0;
                }
            }
        }

        private bool SetStartPoint(int idx)
        {
            for (int x = 0; x < map.Size.x; x++)
            {
                for (int y = 0; y < map.Size.y; y++)
                {
                    Vector2w p = new Vector2w(x, y);
                    if (map.IsMove(p, blockInfo) && moveMap[p.x, p.y] == 0)
                    {
                        moveMap[p.x, p.y] = idx;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool FillAround(int x, int y, int idx, Dictionary<int, int> cnt)
        {
            bool res = false;
            for (int i = 0; i < 4; i++)
            {
                Vector2w p = new Vector2w(x, y) + offset[i];
                if (map.IsMap(p) && map.IsMove(p, blockInfo) && moveMap[p.x, p.y] == 0)
                {
                    moveMap[p.x, p.y] = idx;
                    cnt[idx]++;
                    res = true;
                }
            }
            return res;
        }
    }
}
