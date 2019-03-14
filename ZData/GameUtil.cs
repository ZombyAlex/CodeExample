using System;
using System.Collections.Generic;
using System.IO;
using Lidgren.Network;

namespace ZData
{
	public static class GameUtil
	{
        public static int GetDir(Vector2w pos, Vector2w target)
        {
            if (pos == target)
                return 0;
            int dirX = target.x - pos.x;
            int dirY = target.y - pos.y;
            if (dirY > 0)
                return 0;
            if (dirY < 0)
                return 2;
            if (dirX > 0)
                return 1;
            return 3;
        }

		/*
		public static byte GetDir(Vector2f inPos, Vector2f inTarget)
		{
			if (inPos == inTarget)
			{
				return 0;
			}
			Vector2f aDir = inTarget - inPos;
			if (aDir.y > 0)
			{
				if (aDir.x == 0)
				{
					return 0;
				}
				else if (aDir.x > 0)
				{
					return 1;
				}
				else
					return 7;
			}
			if (aDir.y < 0)
			{
				if (aDir.x == 0)
				{
					return 4;
				}
				else if (aDir.x > 0)
				{
					return 3;
				}
				else
					return 5;
			}
			if (aDir.x > 0)
			{
				return 2;
			}
			return 6;
		}
		public static byte GetDir(Vector2b inPos, Vector2b inTarget)
		{
			if (inPos == inTarget)
			{
				return 0;
			}
			int aDirX = inTarget.x - inPos.x;
			int aDirY = inTarget.y - inPos.y;
			if (aDirY > 0)
			{
				if (aDirX == 0)
				{
					return 0;
				}
				else if (aDirX > 0)
				{
					return 1;
				}
				else
					return 7;
			}
			if (aDirY < 0)
			{
				if (aDirX == 0)
				{
					return 4;
				}
				else if (aDirX > 0)
				{
					return 3;
				}
				else
					return 5;
			}
			if (aDirX > 0)
			{
				return 2;
			}
			return 6;

		}
		

		public static byte GetDir(Vector3w inPos, Vector3w inTarget)
		{
			return GetDir(inPos.ToVector2W(), inTarget.ToVector2W());
		}

		public static byte GetDir(byte inStartDir,sbyte inOffsetDir)
		{
			int aDir = inStartDir;
			aDir += inOffsetDir;
			if (aDir > 7)
				aDir -= 8;
			else if (aDir < 0)
				aDir += 8;
			return (byte) aDir;
		}

		public static sbyte GetOffsetDir(byte inStartDir, byte inTargetDir)
		{
			int aDir = inTargetDir - inStartDir;
			if (aDir > 4)
				aDir -= 8;
			else if (aDir < -4)
				aDir += 8;
			return (sbyte) aDir;
		}

		public static sbyte GetRotateOffset(byte inStartDir, byte inEndDir)
		{
			int aDir = inEndDir - inStartDir;
			if (aDir>4)
			{
				aDir -= 8;
			}
			if (aDir<-4)
			{
				aDir += 8;
			}
			return (sbyte)aDir;
		}
		public static Vector2b GetTarget(Vector2b inPos, byte inDir)
		{
			Vector2b aPos = new Vector2b(inPos.x, inPos.y);
			if (inDir == 0)
				aPos.y++;
			else if (inDir == 1)
			{
				aPos.x++;
				aPos.y++;
			}
			else if (inDir == 2)
				aPos.x++;
			else if (inDir == 3)
			{
				aPos.x++;
				aPos.y--;
			}
			else if (inDir == 4)
				aPos.y--;
			else if (inDir == 5)
			{
				aPos.x--;
				aPos.y--;
			}
			else if (inDir == 6)
				aPos.x--;
			else if (inDir == 7)
			{
				aPos.x--;
				aPos.y++;
			}
			return aPos;
		}
		public static Vector2w GetTarget(Vector2w inPos, byte inDir)
		{
			Vector2w aPos = new Vector2w(inPos.x, inPos.y);
			if (inDir == 0)
				aPos.y++;
			else if (inDir == 1)
			{
				aPos.x++;
				aPos.y++;
			}
			else if (inDir == 2)
				aPos.x++;
			else if (inDir == 3)
			{
				aPos.x++;
				aPos.y--;
			}
			else if (inDir == 4)
				aPos.y--;
			else if (inDir == 5)
			{
				aPos.x--;
				aPos.y--;
			}
			else if (inDir == 6)
				aPos.x--;
			else if (inDir == 7)
			{
				aPos.x--;
				aPos.y++;
			}
			return aPos;
		}
		*/
		public static string ReadString(NetIncomingMessage inMsg)
		{
			int aSize = inMsg.ReadInt32();
			char[] aText = new char[aSize];
			for (int i = 0; i < aSize; i++)
				aText[i] = (char)inMsg.ReadUInt16();
			return new string(aText);
		}

		public static void WriteString(NetOutgoingMessage outMsg, string inStr)
		{
			outMsg.Write((int)inStr.Length);
			for (int i = 0; i < inStr.Length; i++)
				outMsg.Write((ushort)inStr[i]);
		}

		public static string ReadString(BinaryReader inReader)
		{
			int aSize = inReader.ReadInt32();
			char[] aText = new char[aSize];
			for (int i = 0; i < aSize; i++)
				aText[i] = (char)inReader.ReadUInt16();
			return new string(aText);
		}

		public static void WriteString(BinaryWriter outWriter, string inStr)
		{
			outWriter.Write((int)inStr.Length);
			for (int i = 0; i < inStr.Length; i++)
				outWriter.Write((ushort)inStr[i]);
		}

        /*
		public static WRect ReadWRect(NetIncomingMessage inMsg)
		{
			short aX = inMsg.ReadInt16();
			short aY = inMsg.ReadInt16();
			short aW = inMsg.ReadInt16();
			short aH = inMsg.ReadInt16();
			return new WRect(aX, aY, aW, aH);
		}
		public static void WriteWRect(NetOutgoingMessage outMsg, WRect inRect)
		{
			outMsg.Write(inRect.x);
			outMsg.Write(inRect.y);
			outMsg.Write(inRect.w);
			outMsg.Write(inRect.h);
		}

		public static WRectX ReadWRectX(NetIncomingMessage inMsg)
		{
			short aX = inMsg.ReadInt16();
			short aY = inMsg.ReadInt16();
			short aW = inMsg.ReadInt16();
			short aH = inMsg.ReadInt16();
			short aZ = inMsg.ReadInt16();
			return new WRectX(aX, aY, aW, aH, aZ);
		}
		public static void WriteWRectX(NetOutgoingMessage outMsg, WRectX inRect)
		{
			outMsg.Write(inRect.x);
			outMsg.Write(inRect.y);
			outMsg.Write(inRect.w);
			outMsg.Write(inRect.h);
			outMsg.Write(inRect.z);
		}
        */

        /*
		public static Vector2w ReadVector2w(NetIncomingMessage inMsg)
		{
			short aX = inMsg.ReadInt16();
			short aY = inMsg.ReadInt16();
			return new Vector2w(aX, aY);
		}
		public static void WriteVector2w(NetOutgoingMessage outMsg, Vector2w inVec)
		{
			outMsg.Write(inVec.x);
			outMsg.Write(inVec.y);
		}

		public static Vector2w ReadVector2w(BinaryReader inReader)
		{
			short aX = inReader.ReadInt16();
			short aY = inReader.ReadInt16();
			return new Vector2w(aX, aY);
		}
		public static void WriteVector2w(BinaryWriter outWriter, Vector2w inVec)
		{
			outWriter.Write(inVec.x);
			outWriter.Write(inVec.y);
		}

		public static Vector3w ReadVector3w(NetIncomingMessage inMsg)
		{
			short aX = inMsg.ReadInt16();
			short aY = inMsg.ReadInt16();
			short aZ = inMsg.ReadInt16();
			return new Vector3w(aX, aY, aZ);
		}
		public static void WriteVector3w(NetOutgoingMessage outMsg, Vector3w inVec)
		{
			outMsg.Write(inVec.x);
			outMsg.Write(inVec.y);
			outMsg.Write(inVec.z);
		}
		*/
		/*
		public static WRect[] GetRectOffset(WRect inRectStart, WRect inRectEnd)
		{
			int ax = Math.Abs(inRectEnd.x - inRectStart.x);
			int ay = Math.Abs(inRectEnd.y - inRectStart.y);
			int aw = inRectStart.w - ax;
			int ah = inRectStart.h - ay;

			WRect aRect1 = new WRect(inRectEnd.x + aw, inRectEnd.y, ax, inRectEnd.h);
			WRect aRect2 = new WRect(inRectEnd.x, inRectEnd.y + ah, aw, ay);
			if (inRectEnd.x - inRectStart.x < 0)
			{
				aRect1.x = inRectEnd.x;
				aRect2.x = (short)(inRectEnd.x + ax);
			}
			if (inRectEnd.y - inRectStart.y < 0)
			{
				aRect1.y = inRectEnd.y;
				aRect2.y = inRectEnd.y;
			}
			int aCount = 0;
			if (aRect1.h > 0 && aRect1.w > 0)
				aCount++;
			if (aRect2.h > 0 && aRect2.w > 0)
				aCount++;
			WRect[] aRects = new WRect[aCount];
			aCount = 0;
			if (aRect1.h > 0 && aRect1.w > 0)
				aRects[aCount++] = aRect1;
			if (aRect2.h > 0 && aRect2.w > 0)
				aRects[aCount] = aRect2;
			return aRects;
		}
		*/
        /*
		public static WRect[] GetRectOffset(WRect inRectStart, WRect inRectEnd)
		{
			List<WRect> aListRect = new List<WRect>();
			WRect aRectStart = new WRect(inRectStart.x/8, inRectStart.y/8, inRectStart.w/8, inRectStart.h/8);
			WRect aRectEnd = new WRect(inRectEnd.x / 8, inRectEnd.y / 8, inRectEnd.w / 8, inRectEnd.h / 8);

			for (int ix = 0; ix < aRectEnd.w; ix++)
			{
				for (int iy = 0; iy < aRectEnd.h; iy++)
				{
					if (!aRectStart.Contains(new Vector2w(ix+aRectEnd.x, iy+aRectEnd.y)))
					{
						aListRect.Add(new WRect((ix + aRectEnd.x)*8, (iy + aRectEnd.y)*8, 8, 8));
					}
				}
			}
			return aListRect.ToArray();
		}
         */

	    public static int CalcDir(int startDir, int endDir)
	    {
	        return Math.Abs(endDir - startDir);
	    }
	}
}
