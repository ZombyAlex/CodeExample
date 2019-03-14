using System;

#pragma warning disable 0660, 0661

namespace ZData
{
	public class Vector2f
	{
		public float x;
		public float y;
		public Vector2f()
		{
			x = 0;
			y = 0;
		}
		public Vector2f(float inX, float inY)
		{
			x = inX;
			y = inY;
		}

		public static Vector2f operator +(Vector2f inVec1, Vector2f inVec2)
		{
			return new Vector2f(inVec1.x+inVec2.x,inVec1.y+inVec2.y);
		}
		public static Vector2f operator -(Vector2f inVec1, Vector2f inVec2)
		{
			return new Vector2f(inVec1.x - inVec2.x, inVec1.y - inVec2.y);
		}
		public static bool operator ==(Vector2f inVec1, Vector2f inVec2)
		{
			if (inVec1.x == inVec2.x && inVec1.y == inVec2.y)
				return true;
			return false;
		}
		public static bool operator !=(Vector2f inVec1, Vector2f inVec2)
		{
			if (inVec1.x == inVec2.x && inVec1.y == inVec2.y)
				return false;
			return true;
		}
		public void Set(float inX, float inY)
		{
			x = inX;
			y = inY;
		}
		public float GetR(Vector2f inVec)
		{
			if (Math.Abs(x - inVec.x) > Math.Abs(y - inVec.y))
			{
				return Math.Abs(x - inVec.x);
			}
			return Math.Abs(y - inVec.y);
		}
		public void Normalize()
		{
			float aVal = (float)Math.Sqrt(x * x + y * y);
			x = x / aVal;
			y = y / aVal;
		}
	}
}
