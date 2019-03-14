using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZData
{
    public class User : ICloneable
    {
        public uint id;
        public bool isConnection = false;
        public string deviceId;
        public List<Unit> units = new List<Unit>();
        public int money;
        public float timeDisconnect;
        public bool isBattle = false;
        public string name;
        public uint battleId;


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Unit GetSelectUnit()
        {
            return units.Find(f => f.select);
        }
    }
}
