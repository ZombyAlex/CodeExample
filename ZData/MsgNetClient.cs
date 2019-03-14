using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZData
{
    public class MsgClientLoginIdDevice : MsgNetClient
    {
        public string deviceId;

        public MsgClientLoginIdDevice()
        {
        }

        public MsgClientLoginIdDevice(string deviceId)
            : base(EMsgNetClient.loginIdDevice)
        {
            this.deviceId = deviceId;
        }
    }


    public class MsgClientSelectUnit : MsgNetClient
    {
        public string unitId;

        public MsgClientSelectUnit()
        {
        }

        public MsgClientSelectUnit(string unitId) : base(EMsgNetClient.selectUnit)
        {
            this.unitId = unitId;
        }
    }

    public class MsgClientBuyUnit : MsgNetClient
    {
        public string unitId;

        public MsgClientBuyUnit()
        {
        }

        public MsgClientBuyUnit(string unitId)
            : base(EMsgNetClient.buyUnit)
        {
            this.unitId = unitId;
        }
    }

    public class MsgClientStartBattle : MsgNetClient
    {
        public EBattleMode mode;

        public MsgClientStartBattle()
        {
        }

        public MsgClientStartBattle(EBattleMode mode)
            : base(EMsgNetClient.startBattle)
        {
            this.mode = mode;
        }
    }

    public class MsgClientExitPrepareBattle : MsgNetClient
    {
        public MsgClientExitPrepareBattle()
            : base(EMsgNetClient.exitPrepareBattle)
        {
        }
    }

    public class MsgClientUnitMove : MsgNetClient
    {
        public Vector2w pos;
        public MsgClientUnitMove()
            : base(EMsgNetClient.unitMove)
        {
        }

        public MsgClientUnitMove(Vector2w pos)
            : base(EMsgNetClient.unitMove)
        {
            this.pos = pos;
        }
    }

    public class MsgClientUnitRotate : MsgNetClient
    {
        public int dir;
        public MsgClientUnitRotate()
            : base(EMsgNetClient.unitRotate)
        {
        }

        public MsgClientUnitRotate(int dir)
            : base(EMsgNetClient.unitRotate)
        {
            this.dir = dir;
        }
    }

    public class MsgClientUnitShot : MsgNetClient
    {
        public Vector2w pos;
        public MsgClientUnitShot()
            : base(EMsgNetClient.unitShot)
        {
        }

        public MsgClientUnitShot(Vector2w pos)
            : base(EMsgNetClient.unitShot)
        {
            this.pos = pos;
        }
    }
}
