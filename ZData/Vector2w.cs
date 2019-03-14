using System;
using Pathfinding.Serialization.JsonFx;

namespace ZData
{
	public class Vector2w
	{
        public short x;
		public short y;

	    public Vector2w()
	    {
	    }

	    public Vector2w(short inX, short inY)
		{
			x = inX;
			y = inY;
		}

		public Vector2w(int inX, int inY)
		{
			x = (short)inX;
			y = (short)inY;
		}
		public Vector2w(Vector2w inPos)
		{
			x = inPos.x;
			y = inPos.y;
		}

		public static Vector2w operator +(Vector2w inVec1, Vector2w inVec2)
		{
			return new Vector2w((short)(inVec1.x + inVec2.x), (short)(inVec1.y + inVec2.y));
		}
		public static Vector2w operator -(Vector2w inVec1, Vector2w inVec2)
		{
			return new Vector2w((short)(inVec1.x - inVec2.x), (short)(inVec1.y - inVec2.y));
		}
		public static bool operator ==(Vector2w inVec1, Vector2w inVec2)
		{
			//return inVec1.Equals(inVec2);
			if (inVec1.x == inVec2.x && inVec1.y == inVec2.y)
			    return true;
			return false;
		}
		public static bool operator !=(Vector2w inVec1, Vector2w inVec2)
		{
			if (inVec1.x == inVec2.x && inVec1.y == inVec2.y)
				return false;
			return true;
		}
		public void Set(short inX, short inY)
		{
			x = inX;
			y = inY;
		}

		public void Set(int inX, int inY)
		{
			x = (short)inX;
			y = (short)inY;
		}

		public int GetR(Vector2w inVec)
		{
			if (Math.Abs(x - inVec.x) > Math.Abs(y - inVec.y))
			{
				return Math.Abs(x - inVec.x);
			}
			return Math.Abs(y - inVec.y);
		}
        
		public override int GetHashCode()
		{
			unchecked
			{
				return (x.GetHashCode()*497) ^ y.GetHashCode();
			}
		}

	    public bool Equals(Vector2w other)
		{
			return other.x == x && other.y == y;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof(Vector2w)) return false;
			return Equals((Vector2w)obj);
		}
        /*
		public override string ToString()
		{
			return x.ToString() + "," + y;
		}
        */
		public void MoveTo1(Vector2w inTarget)
		{
			if (x < inTarget.x)
				x++;
			else if (x > inTarget.x)
				x--;
			if (y < inTarget.y)
				y++;
			else if (y > inTarget.y)
				y--;
		}

		public bool IsZero()
		{
			if(x!=0 || y!=0)
				return false;
			return true;
		}

		public bool IsBeside(Vector2w p)
		{
			if (p.x != x && p.y != y)
				return false;
			if (GetR(p) != 1)
				return false;
			return true;
		}
	}
}
