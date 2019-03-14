using System;
using System.IO;
using System.Threading;
using ZData;
using ZServer.Tests;


namespace ZServer
{
	class Program
	{
		static void Main(string[] args)
		{
			//TestJson();


            if (File.Exists("close.txt"))
            {
                File.Delete("close.txt");
            }

            WriteLog("start");

            Console.WriteLine("Proccesor count = " + Environment.ProcessorCount);
            Console.WriteLine("Platform = " + Environment.OSVersion.Platform);

            bool aIsWindows = Environment.OSVersion.Platform.ToString() == "Win32NT";
            

#if DEBUG_TRY
		    try
		    {
#endif

		        //StreamReader reader = new StreamReader("data/items.xml");
		        //ItemInfo.Instance.Init(reader.ReadToEnd());
		        //reader.Close();

                GameData.Init();

		        ServerDataBase dataGame = new ServerDataBase(20, "DataBase");
		        Thread dataGameThread = new Thread(dataGame.Run) {Name = "DataGame"};

		        ServerGame game = new ServerGame(20, "Game");
		        Thread gameThread = new Thread(game.Run) {Name = "Game"};

		        ServerNet netGame = new ServerNet(20, "Net");
		        Thread netGameThread = new Thread(netGame.Run) {Name = "Net"};



		        dataGameThread.Start();

		        gameThread.Start();

		        netGameThread.Start();



		        Console.WriteLine("Server is ready");

		        while (true)
		        {
		            if (Data.stopServer)
		                break;

		            Thread.Sleep(500);

		            if (aIsWindows)
		            {
		                string aCommand = Console.ReadLine();
		                if (aCommand == "exit")
		                {
		                    break;
		                }
		                else if (aCommand == "test")
		                {
		                    /*
                        MsgSingle msgSingle = new MsgSingle(EMsgSingle.loginDone);
                        msgSingle.SetType(EMsg.single);
                        msgSingle.SetId(2);
                        TransportGameNet.instance.inMessages.Add(msgSingle);
                         */
		                }
		            }
		        }

		        netGame.Terminate();
		        netGameThread.Join();

		        game.Terminate();
		        gameThread.Join();

		        dataGame.Terminate();
		        dataGameThread.Join();




		        StreamWriter aWriter = new StreamWriter("close.txt", true);
		        aWriter.WriteLine("0");
		        aWriter.Close();

		        WriteLog("close");


#if DEBUG_TRY
		    }
		    catch (Exception ex)
		    {
		        Tools.SaveCrash(ex.ToString(), true);
		    }
#endif
		}

        public static void WriteLog(string inText)
        {
            StreamWriter aWriter = new StreamWriter("start.txt", true);
            aWriter.WriteLine("------------------------------------------------------------------------------------------------");
            aWriter.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + inText);
            aWriter.Close();
        }



		static void Tests()
		{
			EventChannelTest test = new EventChannelTest();
			test.Test();
		}

		static void TestJson()
		{
			TestJson test = new TestJson();
			test.Test();
		}
	}
}
