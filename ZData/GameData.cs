using System.IO;
using Pathfinding.Serialization.JsonFx;

namespace ZData
{
    public static class GameData
    {
        public static UnitInfo unitInfo;
        public static ShellInfo shellInfo;
        public static BlockInfo blockInfo;

        public static void Init()
        {
            StreamReader reader = new StreamReader("data/units.json");
            string json = reader.ReadToEnd();
            reader.Close();
            unitInfo = JsonReader.Deserialize<UnitInfo>(json);
            unitInfo.Init();


            reader = new StreamReader("data/shells.json");
            json = reader.ReadToEnd();
            reader.Close();
            shellInfo = JsonReader.Deserialize<ShellInfo>(json);
            shellInfo.Init();


            reader = new StreamReader("data/blocks.json");
            json = reader.ReadToEnd();
            reader.Close();
            blockInfo = JsonReader.Deserialize<BlockInfo>(json);
            blockInfo.Init();
        }
    }
}
