using System;
using System.Diagnostics;
using System.IO;

namespace ZServer
{
	public class Tools
	{
		public static void SaveCrash(string text, bool isStopServer)
		{
			using (StreamWriter writer = File.AppendText("crash.txt"))
			{
				writer.WriteLine("------------------------------------------------------------------------------------------------------------");
				writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + text);
			    if (isStopServer)
			        Data.stopServer = true;
			}
		}

		public static void MegaLog(string threadName, string text)
		{
			File.WriteAllText("mega_log_" + threadName + ".txt", text);
		}

		public static void Log(string filename, string text)
		{
			using (StreamWriter writer = File.AppendText(filename))
			{
				writer.WriteLine(text);
			}
		}


		public delegate void ProcFunction();

		public static void CallFunc(ProcFunction func, string filename)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			func();
			sw.Stop();
			WriteLogTime(sw.ElapsedMilliseconds, func.Method.Name, filename);
		}

		private static void WriteLogTime(long timeMilliseconds, string funcName, string filename)
		{
			if (timeMilliseconds >= 1000)
				Log(filename, "f=" + funcName + " time = " + timeMilliseconds / 1000.0);
			else if (timeMilliseconds >= 100)
				Log(filename, "f=" + funcName + " time = " + timeMilliseconds / 1000.0);
			//else if (timeMilliseconds >= 50)
			//	Util.WriteLog("time_sheduleL.txt", "f=" + funcName + " time = " + timeMilliseconds / 1000.0);
		}

		public delegate void ProcFunctionUpdate(float dt);

		public static void CallFunc(ProcFunctionUpdate func, float dt, string filename)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			func(dt);
			sw.Stop();
			WriteLogTime(sw.ElapsedMilliseconds, func.Method.Name, filename);
		}
	}
}
