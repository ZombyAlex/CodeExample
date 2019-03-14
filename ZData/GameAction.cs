
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

namespace ZData
{
    public enum EGameAction
    {
        visibleUnit,
        activateTurn,
        selectUnit,
        moveUnit,
        rotateUnit,
        infoUnit,
        shotUnit,
        damagePos,
        destroyUnit
    }

    public class GameAction
    {
        public EGameAction actionType;
        public string json;
        
        public void Write()
        {
            json = JsonWriter.Serialize(this);
        }

        public virtual Vector2w GetPos()
        {
            return null;
        }
    }

    public class GameActionVisible : GameAction
    {
        public uint unitUID;
        public bool visible;
        public Vector2w pos;
        public int dir;
        public string unitType;

        public GameActionVisible()
        {
            actionType = EGameAction.visibleUnit;
        }

        public GameActionVisible(Unit unit, bool visible)
        {
            actionType = EGameAction.visibleUnit;
            this.visible = visible;
            unitUID = unit.uid;
            pos = unit.pos;
            dir = unit.dir;
            unitType = unit.id;
        }

        public override Vector2w GetPos()
        {
            return pos;
        }
    }

    public class GameActionActivateTurn : GameAction
    {
        public bool activate;

        public GameActionActivateTurn()
        {
            actionType = EGameAction.activateTurn;
        }

        public GameActionActivateTurn(bool activate)
        {
            actionType = EGameAction.activateTurn;
            this.activate = activate;
        }
    }

    public class GameActionSelectUnit : GameAction
    {
        public uint unitUID;
        public bool isSelect;

        public GameActionSelectUnit()
        {
            actionType = EGameAction.selectUnit;
        }

        public GameActionSelectUnit(uint unitUid, bool isSelect)
        {
            actionType = EGameAction.selectUnit;
            unitUID = unitUid;
            this.isSelect = isSelect;
        }
    }

    public class GameActionMoveUnit : GameAction
    {
        public uint unitUID;
        public Vector2w pos;

        public GameActionMoveUnit()
        {
            actionType = EGameAction.moveUnit;
        }

        public GameActionMoveUnit(uint unitUid, Vector2w pos)
        {
            actionType = EGameAction.moveUnit;
            unitUID = unitUid;
            this.pos = new Vector2w(pos);
        }

        public override Vector2w GetPos()
        {
            return pos;
        }
    }

    public class GameActionRotateUnit : GameAction
    {
        public uint unitUID;
        public Vector2w pos;
        public int dir;

        public GameActionRotateUnit()
        {
            actionType = EGameAction.rotateUnit;
        }

        public GameActionRotateUnit(uint unitUid, int dir, Vector2w pos)
        {
            actionType = EGameAction.rotateUnit;
            unitUID = unitUid;
            this.pos = new Vector2w(pos);
            this.dir = dir;
        }

        public override Vector2w GetPos()
        {
            return pos;
        }
    }

    public class GameActionInfoUnit : GameAction
    {
        public Unit unit;

        public GameActionInfoUnit()
        {
            actionType = EGameAction.infoUnit;
        }

        public GameActionInfoUnit(Unit unit)
        {
            actionType = EGameAction.infoUnit;
            this.unit = unit;
        }
    }

    public class GameActionShotUnit : GameAction
    {
        public uint unitUID;
        public Vector2w pos;

        public GameActionShotUnit()
        {
            actionType = EGameAction.shotUnit;
        }

        public GameActionShotUnit(uint unitUid, Vector2w pos)
        {
            actionType = EGameAction.shotUnit;
            unitUID = unitUid;
            this.pos = pos;
        }
    }

    public class GameActionDamagePos : GameAction//TODO добавить выбор повреждения брони или здоровья танка
    {
        public Vector2w pos;
        public int damage;

        public GameActionDamagePos()
        {
            actionType = EGameAction.damagePos;
        }

        public GameActionDamagePos(int damage, Vector2w pos)
        {
            actionType = EGameAction.damagePos;
            this.damage = damage;
            this.pos = pos;
        }
    }

    public class GameActionDestroyUnit : GameAction
    {
        public uint unitUID;

        public GameActionDestroyUnit()
        {
            actionType = EGameAction.destroyUnit;
        }

        public GameActionDestroyUnit(uint unitUid)
        {
            actionType = EGameAction.destroyUnit;
            unitUID = unitUid;
        }
    }
}
