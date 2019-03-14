using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ZData;

namespace ZServer
{
	public class GameThread
	{
		private bool isTerminate;
		private List<Shedule> sheduleList = new List<Shedule>();
		private int sleepWait;
		private string threadName;


		public GameThread(int sleepWait, string threadName)
		{
			this.sleepWait = sleepWait;
			this.threadName = threadName;
		}

	    public void Run()
	    {

	        Init();

	        XTimer timer = new XTimer();

	        while (!isTerminate)
	        {

#if DEBUG_TRY
	            try
	            {
#endif
	                float dt = (float) timer.DeltaTime();


	                for (int i = 0; i < sheduleList.Count; i++)
	                {
	                    sheduleList[i].Update(dt);
	                }

	                Update(dt);

	                Thread.Sleep(sleepWait);
#if DEBUG_TRY
	            }
	            catch (Exception ex)
	            {
	                Tools.SaveCrash(ex.ToString(), false);
	            }
#endif
	        }
	        OnTerminate();
	        Console.WriteLine("Stop " + threadName);

	    }

	    protected virtual void Init()
		{
		}

		protected virtual void Update(float dt)
		{
		}

		protected virtual void OnTerminate()
		{
		}

		public void Terminate()
		{
			Console.WriteLine("Terminate " + threadName);
			isTerminate = true;
		}


		protected void WriteLog(string text)
		{
			StreamWriter aWriter = new StreamWriter("logs/log_" + threadName + ".txt", true);
			aWriter.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + text);
			aWriter.Close();
		}

	    protected void AddSheduleCall(float time, ProcFunction function, float offsetTime)
	    {
	        sheduleList.Add(new SheduleCall(time, function, offsetTime));
	    }

        protected void AddSheduleUpdate(float time, ProcFunctionUpdate function, float offsetTime)
        {
            sheduleList.Add(new SheduleUpdate(time, function, offsetTime));
        }

	}
}
