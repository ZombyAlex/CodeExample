using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ZServer
{

	public class Shedule
	{
		protected float time;
		protected float curTime = 0.0f;
		public Shedule(float inTime, float inOffsetTime)
		{
			time = inTime;
			curTime = inOffsetTime;
		}
		public virtual void Update(float dt)
		{}

		protected void WriteLogTime(long timeMilliseconds, string funcName)
		{
			if (timeMilliseconds >= 1000)
				Tools.Log("logs/time_sheduleH.txt", "f=" + funcName + " time = " + timeMilliseconds / 1000.0);
			else if (timeMilliseconds >= 100)
				Tools.Log("logs/time_sheduleM.txt", "f=" + funcName + " time = " + timeMilliseconds / 1000.0);
			//else if (timeMilliseconds >= 50)
			//	Util.WriteLog("time_sheduleL.txt", "f=" + funcName + " time = " + timeMilliseconds/1000.0);
		}
	}

	public delegate void ProcFunction();

	public class SheduleCall : Shedule
	{
		private readonly ProcFunction func;
		private bool debug = false;
		public SheduleCall(float inTime, ProcFunction inFunc, float inOffsetTime, bool inDebug = false) 
			: base(inTime, inOffsetTime)
		{
			debug = inDebug;
			func = inFunc;
			debug = false;
		}

		public override void Update(float dt)
		{
			curTime += dt;
			if (curTime >= time)
			{
				curTime -= time;
				if (debug)
					Tools.MegaLog("shedule.txt", "s=" + func.Method.Name);
				Stopwatch sw = new Stopwatch();
				sw.Start();
				func();
				sw.Stop();
				WriteLogTime(sw.ElapsedMilliseconds, func.Method.Name);
				if (debug)
					Tools.MegaLog("shedule.txt", "d=" + func.Method.Name);
			}
		}
	}

	public delegate void ProcFunctionUpdate(float dt);

	public class SheduleUpdate: Shedule
	{
		private readonly ProcFunctionUpdate func;
		private bool debug = false;
		public SheduleUpdate(float inTime, ProcFunctionUpdate inFunc, float inOffsetTime, bool inDebug = false)
			: base(inTime, inOffsetTime)
		{
			debug = inDebug;
			func = inFunc;
			debug = false;
		}

		public override void Update(float dt)
		{
			curTime += dt;
			if (curTime >= time)
			{
				curTime -= time;
				if (debug)
					Tools.MegaLog("shedule.txt", "s=" + func.Method.Name);
				Stopwatch sw = new Stopwatch();
				sw.Start();
				func(time);
				sw.Stop();
				WriteLogTime(sw.ElapsedMilliseconds, func.Method.Name);
				if (debug)
					Tools.MegaLog("shedule.txt", "d=" + func.Method.Name);
			}
		}
	}
}
