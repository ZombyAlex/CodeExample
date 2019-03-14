using System.Diagnostics;

namespace ZData
{
	public class XTimer
	{
		private Stopwatch timer;
		private double memDt;
		public XTimer()
		{
			timer = new Stopwatch();
			timer.Start();
			memDt = timer.ElapsedTicks/(double)Stopwatch.Frequency;
		}
		public double DeltaTime()
		{
			double aDt = timer.ElapsedTicks / (double)Stopwatch.Frequency;
			double aVal = aDt - memDt;
			memDt = aDt;
			return aVal;
		}
	}
}
