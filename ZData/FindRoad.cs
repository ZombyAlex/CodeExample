using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZData
{
    public class FindRoad
    {
        private Vector2w size;
        private int[,] findMap;
        private Vector2w[] offset = new[] { new Vector2w(1, 0), new Vector2w(0, 1), new Vector2w(-1, 0), new Vector2w(0, -1) };

        public FindRoad(Vector2w size)
        {
            this.size = size;
            findMap = new int[size.x, size.y];
        }

        public bool Find(Vector2w startPos, Vector2w endPos, Map map, BlockInfo blockInfo, List<Vector2w> outRoad)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    findMap[x, y] = 1001;
                }
            }

            findMap[startPos.x, startPos.y] = 1000;
            findMap[endPos.x, endPos.y] = 0;

            int index = 0;

            bool res = false;
            bool exit = false;
            do
            {
                res = false;
                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        if (findMap[x, y] == index)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Vector2w p = new Vector2w(x + offset[i].x, y + offset[i].y);
                                if (map.IsMap(p))
                                {
                                    if (findMap[p.x, p.y] == 1000)
                                    {
                                        exit = true;
                                        break;
                                    }
                                    if (findMap[x, y] == 1001 && map.IsMove(p, blockInfo))
                                    {
                                        findMap[p.x, p.y] = index + 1;
                                        res = true;
                                    }
                                }
                            }
                        }
                        if(exit)
                            break;
                    }
                    if (exit)
                        break;
                }
                if (exit)
                    break;
                index++;
            } while (res);

            outRoad.Add(startPos);

            int memIndex = 1000;
            Vector2w pos = new Vector2w(startPos);
            do
            {
                int memDir = -1;
                for (int i = 0; i < 4; i++)
                {
                    Vector2w p = pos + offset[i];
                    if (map.IsMap(p))
                    {
                        if (findMap[p.x, p.y] < memIndex)
                        {
                            memIndex = findMap[p.x, p.y];
                            memDir = i;
                        }
                    }
                }
                if (memDir == -1)
                    return false;
                pos = pos + offset[memDir];
                outRoad.Add(new Vector2w(pos));
                if (outRoad.Count > 500)
                    return true;
            } while (memIndex != 0);
            
            return true;
        }
    }
}
