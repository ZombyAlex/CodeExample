using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZData
{
    public static class VisiblePoint
    {
        public static bool IsVisible(Vector2w posStart, Vector2w posEnd, Map map, BlockInfo blockInfo)
        {
            Vector2f startPos = new Vector2f(posStart.x + 0.5f, posStart.y + 0.5f);
            Vector2f endPos = new Vector2f(posEnd.x + 0.5f, posEnd.y + 0.5f);
            return TraceRayVisible(startPos, endPos, posStart.GetR(posEnd), map, blockInfo);
        }

        private static bool TraceRayVisible(Vector2f inStart, Vector2f inEnd, int inLength, Map map, BlockInfo blockInfo)
        {
            Vector2f aDir = inEnd - inStart;
            float ax;
            float ay;
            if (Math.Abs(aDir.x) > Math.Abs(aDir.y))
            {
                if (Math.Abs(aDir.y) > 0.001f)
                    ay = aDir.y / Math.Abs(aDir.x);
                else
                    ay = 0;
                ax = 1.0f;
                if (aDir.x < 0)
                    ax = -1.0f;
            }
            else
            {
                if (Math.Abs(aDir.x - 0.0f) > 0.001f)
                    ax = aDir.x / Math.Abs(aDir.y);
                else
                    ax = 0;
                ay = 1.0f;
                if (aDir.y < 0)
                    ay = -1.0f;
            }
            Vector2f aCurPos = new Vector2f(inStart.x, inStart.y);
            int aCountLen = 0;
            while (true)
            {
                aCurPos.x += ax;
                aCurPos.y += ay;

                Vector2w aPos = new Vector2w((int)Math.Floor(aCurPos.x), (int)Math.Floor(aCurPos.y));

                if (!map.IsMap(aPos))
                    return false;
                aCountLen++;
                if (aCountLen >= inLength)
                    return true;
                if (!map.IsVisibleBlock(aPos, blockInfo))
                    return false;
            }
        }

        public static bool TraceRayShot(Vector2f posStart, Vector2f posEnd, int length, out List<Vector2f> outPosCollisions, bool isThrow, Map map, BlockInfo blockInfo)
        {
            outPosCollisions = new List<Vector2f>();
            Vector2f aDir = posEnd - posStart;
            float ax;
            float ay;
            if (Math.Abs(aDir.x) > Math.Abs(aDir.y))
            {
                if (Math.Abs(aDir.y) > 0.001f)
                    ay = aDir.y / Math.Abs(aDir.x);
                else
                    ay = 0;
                ax = 1.0f;
                if (aDir.x < 0)
                    ax = -1.0f;
            }
            else
            {
                if (Math.Abs(aDir.x - 0.0f) > 0.001f)
                    ax = aDir.x / Math.Abs(aDir.y);
                else
                    ax = 0;
                ay = 1.0f;
                if (aDir.y < 0)
                    ay = -1.0f;
            }
            Vector2f aCurPos = new Vector2f(posStart.x, posStart.y);
            int aCountLen = 0;
            while (true)
            {
                aCurPos.x += ax;
                aCurPos.y += ay;

                Vector2w pos = new Vector2w((int)Math.Floor(aCurPos.x), (int)Math.Floor(aCurPos.y));
                if (!map.IsMap(pos))
                {
                    if (isThrow)
                    {
                        aCurPos.x -= ax;
                        aCurPos.y -= ay;
                    }
                    outPosCollisions.Add(aCurPos);
                    return false;
                }
                
                if (!IsShotFree(pos, isThrow, map, blockInfo))
                {
                    if (!isThrow)
                    {
                        outPosCollisions.Add(aCurPos);
                        return true;
                    }
                    else
                    {
                        aCurPos.x -= ax;
                        aCurPos.y -= ay;
                        outPosCollisions.Add(aCurPos);
                        return true;
                    }
                }
                if (map.IsUnitPos(pos))
                {
                    outPosCollisions.Add(aCurPos);
                }
                aCountLen++;
                if (aCountLen >= length)
                {
                    outPosCollisions.Add(aCurPos);
                    return false;
                }
            }
        }

        static bool IsShotFree(Vector2w pos, bool isThrow, Map map, BlockInfo blockInfo)
        {
            if (!isThrow && map.IsUnitPos(pos))
                return true;
            if (!map.IsMoveBlock(pos, blockInfo))
                return false;
            return true;
        }
    }
}
