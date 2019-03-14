using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace ZData
{
    public class UserNetData
    {
        public string steamId;
        public string steamUID;
        public uint id;
        public string name;
        //public string pass;
        public NetConnection address;
        //public EUserDataState state;
        public bool connected;
        public uint idClan;
        //public EUserRights adm;

        private bool isBan;

        public List<double> timeMgs = new List<double>();

        public UserNetData(string inSteamId, string inSteamUID, uint inId, string inName,
            NetConnection inAddress, uint inIdClan, bool inIsBan)
        {
            steamId = inSteamId;
            steamUID = inSteamUID;
            id = inId;
            name = inName;
            address = inAddress;
            //state = inState;
            connected = true;
            idClan = inIdClan;
            //adm = inRights;
            isBan = inIsBan;
        }

        public void SetBan(bool inBan)
        {
            isBan = inBan;
        }

        public bool GetBan()
        {
            return isBan;
        }
    }
}
