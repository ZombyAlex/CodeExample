using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZData;

namespace ZServer.Game
{
    public class ZBattleManager
    {
        private ZGameData d;


        public ZBattleManager(ZGameData d)
        {
            this.d = d;
        }

        public void UnitMove(uint userId, Vector2w pos)
        {
            User user = d.GetUser(userId);
            Unit unit = user.GetSelectUnit();
            Battle battle = d.GetBattle(user.battleId);
            if (!battle.IsUserTurn(userId))
                return;
            List<Vector2w> road = new List<Vector2w>();
            if (battle.findRoad.Find(unit.pos, pos, battle.GetMap(), GameData.blockInfo, road))
            {
                ClampRoad(road, unit.ap, unit.dir, GameData.unitInfo[unit.id]);
                if (road.Count > 1)//move
                {
                    UnitVisible unitVisible = CreateUnitVisible(userId, unit.pos, battle);
                    battle.GetMap().RemoveUnitPos(unit.pos);
                    List<GameAction> actions = new List<GameAction>();
                    for (int i = 1; i < road.Count; i++)
                    {
                        int direction = GameUtil.GetDir(road[i - 1], road[i]);
                        if (unit.dir != direction)
                        {
                            unit.ap -= GameUtil.CalcDir(unit.dir, direction) * GameData.unitInfo[unit.id].apRotate;
                            unit.dir = direction;
                            actions.Add(new GameActionRotateUnit(unit.uid, unit.dir, unit.pos));
                        }
                        unit.ap -= GameData.unitInfo[unit.id].apMove;
                        unit.pos = road[i];
                        actions.Add(new GameActionMoveUnit(unit.uid, unit.pos));
                        
                        //проверка обнаруженных юнитов и остановка
                        if (IsNewUnitVisible(unitVisible, userId, unit.pos, battle))
                            break;
                    }
                    
                    battle.GetMap().SetUnitPos(unit.pos);

                    SendAllMoveUnit(userId, unit, battle, actions);
                    SendAllInfoUnit(unit, battle);

                    //обновление видимости вражеских юнитов
                    UpdateEnemyVisible(userId, unit.pos, battle);
                }
            }
        }

        private void UpdateEnemyVisible(uint userId, Vector2w pos, Battle battle)
        {
            var data = battle.GetData();
            for (int i = 0; i < data.users.Count; i++)
            {
                if (data.users[i].userid != userId)
                {
                    Unit unit = d.GetBattleUnit(userId);
                    bool isVisible = VisiblePoint.IsVisible(pos, unit.pos, battle.GetMap(), GameData.blockInfo);

                    Data.t.netMsg.Add(new MsgServerGameAction(userId, new GameActionVisible(unit, isVisible)));
                }
            }
        }

        private UnitVisible CreateUnitVisible(uint userId, Vector2w pos, Battle battle)
        {
            UnitVisible unitVisible = new UnitVisible();
            var data = battle.GetData();
            for (int i = 0; i < data.users.Count; i++)
            {
                if (data.users[i].userid != userId)
                {
                    Vector2w unitPos = d.GetBattleUnitPos(userId);
                    if (VisiblePoint.IsVisible(pos, unitPos, battle.GetMap(), GameData.blockInfo))
                        unitVisible.AddVisible(data.users[i].userid);
                }
            }
            return unitVisible;
        }

        private bool IsNewUnitVisible(UnitVisible unitVisible, uint userId, Vector2w pos, Battle battle)
        {
            var data = battle.GetData();
            for (int i = 0; i < data.users.Count; i++)
            {
                if (data.users[i].userid != userId)
                {
                    Vector2w unitPos = d.GetBattleUnitPos(userId);
                    if (VisiblePoint.IsVisible(pos, unitPos, battle.GetMap(), GameData.blockInfo) && !unitVisible.IsVisible(data.users[i].userid))
                        return true;
                        
                }
            }
            return false;
        }

        private void SendAllMoveUnit(uint userId, Unit unit, Battle battle, List<GameAction> actions)
        {
            BattleData data = battle.GetData();
            for (int i = 0; i < data.users.Count; i++)
            {
                List<GameAction> list = new List<GameAction>();
                if (data.users[i].userid == userId)
                    list.AddRange(actions);
                else
                {
                    bool isVisible = false;
                    User user = d.GetUser(data.users[i].userid);
                    Unit unitEnemy = user.GetSelectUnit();
                    for (int j = 0; j < actions.Count; j++)
                    {
                        if (VisiblePoint.IsVisible(unitEnemy.pos, actions[i].GetPos(), battle.GetMap(), GameData.blockInfo))
                        {
                            if (!isVisible)
                            {
                                list.Add(new GameActionVisible(unit, true));
                                isVisible = true;
                            }
                            
                        }
                    }
                }
                Data.t.netMsg.Add(new MsgServerGameActionList(data.users[i].userid, list));
            }
        }

        private void SendAllInfoUnit(Unit unit, Battle battle)
        {
            BattleData data = battle.GetData();
            for (int i = 0; i < data.users.Count; i++)
            {
                Data.t.netMsg.Add(new MsgServerGameAction(data.users[i].userid, new GameActionInfoUnit(unit)));
            }
        }

        private void ClampRoad(List<Vector2w> road, int ap, int dir, UnitInfoItem unitInfo)
        {
            int curDir = dir;
            int count = 0;
            for (int i = 0; i < road.Count - 1; i++)
            {
                int direction = GameUtil.GetDir(road[i], road[i + 1]);
                if (direction != curDir)
                {
                    ap -= GameUtil.CalcDir(curDir, direction)*unitInfo.apRotate;
                    if (ap < 0)
                        break;
                }
                ap -= unitInfo.apMove;
                if (ap < 0)
                    break;
                count++;
            }

            road.RemoveRange(count, road.Count - count);
        }

        public void UnitRotate(uint userId, int dir)
        {
            User user = d.GetUser(userId);
            Unit unit = user.GetSelectUnit();
            Battle battle = d.GetBattle(user.battleId);
            if(!battle.IsUserTurn(userId))
                return;
            int ap = GameUtil.CalcDir(unit.dir, dir)*GameData.unitInfo[unit.id].apRotate;
            if (unit.ap < ap)
                return;
            unit.ap -= ap;
            unit.dir = dir;
            BattleData data = battle.GetData();
            for (int i = 0; i < data.users.Count; i++)
            {
                Vector2w pos = d.GetBattleUnitPos(data.users[i].userid);
                if (data.users[i].userid == userId || VisiblePoint.IsVisible(unit.pos, pos, battle.GetMap(), GameData.blockInfo))
                {
                    Data.t.netMsg.Add(new MsgServerGameAction(data.users[i].userid, new GameActionRotateUnit(unit.uid, unit.dir, unit.pos)));
                    Data.t.netMsg.Add(new MsgServerGameAction(data.users[i].userid, new GameActionInfoUnit(unit)));
                }
            }
        }

        public void UnitShot(uint userId, Vector2w pos)
        {
            User user = d.GetUser(userId);
            Unit unit = user.GetSelectUnit();
            Battle battle = d.GetBattle(user.battleId);
            if (!battle.IsUserTurn(userId))
                return;
            Vector2f startPos = new Vector2f(unit.pos.x + 0.5f, unit.pos.y + 0.5f);
            Vector2f endPos = new Vector2f(pos.x + 0.5f, pos.y + 0.5f);
            List<Vector2f> list = new List<Vector2f>();
            if (VisiblePoint.TraceRayShot(startPos, endPos, 64, out list, false, battle.GetMap(), GameData.blockInfo))
            {
                if (list.Count > 0)
                {
                    Vector2w p = new Vector2w((short) list[0].x, (short)list[0].y);
                    int damage = 10;
                    int armorPiercing = 10;
                    
                    SendAllShot(userId, pos, battle, unit);
                    DamagePoint(p, damage, armorPiercing);
                }
            }


        }

        private void SendAllShot(uint userId, Vector2w pos, Battle battle, Unit unit)
        {
            BattleData data = battle.GetData();
            for (int i = 0; i < data.users.Count; i++)
            {
                Vector2w posEnemy = d.GetBattleUnitPos(data.users[i].userid);
                if (data.users[i].userid == userId || VisiblePoint.IsVisible(posEnemy, unit.pos, battle.GetMap(), GameData.blockInfo)
                    || VisiblePoint.IsVisible(posEnemy, pos, battle.GetMap(), GameData.blockInfo))
                {
                    Data.t.netMsg.Add(new MsgServerGameAction(data.users[i].userid, new GameActionShotUnit(unit.uid, unit.pos)));
                    Data.t.netMsg.Add(new MsgServerGameAction(data.users[i].userid, new GameActionInfoUnit(unit)));
                }
            }
        }

        private void DamagePoint(Vector2w pos, int damage, int armorPiercing)
        {
            //TODO
        }


        public void UpdateBattles(float dt)
        {
            d.UpdateBattles(dt);
        }


        
    }
}
