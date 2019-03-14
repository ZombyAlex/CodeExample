
using System.Collections.Generic;

namespace ZData
{
    public class UnitVisible
    {
        private List<uint> visible = new List<uint>();


        public void AddVisible(uint id)
        {
            visible.Add(id);
        }

        public void RemoveVisible(uint id)
        {
            visible.Remove(id);
        }

        public bool IsVisible(uint id)
        {
            for (int i = 0; i < visible.Count; i++)
            {
                if (visible[i] == id)
                    return true;
            }
            return false;
        }
    }
}
