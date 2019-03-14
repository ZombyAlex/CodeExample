
namespace ZData
{
    public class Unit
    {
        public uint uid;
        public string id;
        public int health;
        public int ap;
        public Vector2w pos;
        public int dir;

        //additional
        public bool select = false;
        

        public Unit()
        {
        }

        public Unit(string unitId, uint uid)
        {
            this.uid = uid;
            id = unitId;
            health = GameData.unitInfo[unitId].health;
            ap = GameData.unitInfo[unitId].ap;
        }

        public void RestoreUnitBattle()
        {
            ap = GameData.unitInfo[id].ap;
        }
    }
}
