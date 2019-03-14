using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZData
{
    public enum EMsgNetClient
    {
        loginIdDevice,
        selectUnit,
        buyUnit,
        startBattle,
        exitPrepareBattle,
        unitMove,
        unitRotate,
        unitShot
    }

    public enum EMsgNetServer
    {
        user,
        selectUnit,
        notification,
        prepareBattle,
        createBattle,
        battleData,
        gameAction,
        gameActionList
    }
    
}
