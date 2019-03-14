using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZData
{
    public class UserInfo
    {
        private uint id;
        private string deviceId;

        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
