using System;
using System.Collections.Generic;
using Lidgren.Network;
using ZData;
using ZServer.Game;

namespace ZServer
{
    public class ServerNet: GameThread
    {
        private NetPeerConfiguration config;
        private NetServer server;
        private ZNet net = null;

        public ServerNet(int sleepWait, string threadName) : base(sleepWait, threadName)
        {
            
        }

        protected override void Init()
        {
            InitServer();

            net = new ZNet(server);

            AddSheduleCall(0.1f, net.UpdateOutMsg, 0.05f);
        }

        private void InitServer()
        {
            string nameServer = GameConst.serverName;
            int port = GameConst.port;
            config = new NetPeerConfiguration(nameServer) {Port = port};
            config.MaximumConnections = 2048;
            server = new NetServer(config);
            server.Start();
            Console.WriteLine("MaxConnections=" + config.MaximumConnections);
        }

        protected override void Update(float dt)
        {
            NetIncomingMessage msg;
            while ((msg = server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        WriteLog("Error net message=" + msg.ReadString());

                        break;
                    case NetIncomingMessageType.Data:
                        
                        net.ProcMsg(msg);

                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        string reason = msg.ReadString();
                        WriteLog("Status Changed ip = " + msg.SenderConnection.RemoteUniqueIdentifier + " status=" + status + " reason:" + reason);

                        if (status == NetConnectionStatus.Connected)
                        {
                            WriteLog("connect ip = " + msg.SenderConnection.RemoteUniqueIdentifier);

                            net.ChangeConnectStatusClient(msg.SenderConnection, true);
                        }
                        if (status == NetConnectionStatus.Disconnected)
                        {
                            net.ChangeConnectStatusClient(msg.SenderConnection, false);
                        }

                        WriteLog("num connect = " + server.Connections.Count);
                        Console.WriteLine("connection = " + server.Connections.Count);
                        break;
                    default:
                        WriteLog("Unhandled type = " + msg.MessageType);
                        break;
                }
                server.Recycle(msg);
            }
        }

        

        
    }
}
