using System.Collections.Generic;
using System.IO;
using Lidgren.Network;
using Pathfinding.Serialization.JsonFx;
using ZData;

namespace ZServer.Game
{
    public class UserInfoList
    {
        public List<UserInfo> userInfo = new List<UserInfo>();
        [JsonIgnore] public bool isChange = false;
        [JsonIgnore] public Dictionary<string, UserInfo> userDeviceId = new Dictionary<string, UserInfo>();
        [JsonIgnore] public Dictionary<uint, UserInfo>  users = new Dictionary<uint, UserInfo>();

        public void AddInfo(UserInfo info)
        {
            userInfo.Add(info);
            userDeviceId.Add(info.DeviceId, info);
            isChange = true;
        }

        public void UpdatePostLoad()
        {
            foreach (UserInfo it in userInfo)
            {
                userDeviceId.Add(it.DeviceId, it);
                users.Add(it.Id, it);
            }
        }

    }

    public class ZDataBaseData
    {
        private UserInfoList infoList;

        public ZDataBaseData()
        {
            Load();
        }

        private void Load()
        {
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            if (File.Exists("data/user_list.txt"))
            {
                StreamReader reader = new StreamReader("data/user_list.txt");
                string json = reader.ReadToEnd();
                reader.Close();
                infoList = JsonReader.Deserialize<UserInfoList>(json);
                infoList.UpdatePostLoad();
            }
            else
            {
                infoList = new UserInfoList();
            }
        }

        public void Save()
        {
            if (infoList.isChange)
            {
                infoList.isChange = false;
                SaveUserInfo();
            }
        }

        private void SaveUserInfo()
        {
            StreamWriter writer = new StreamWriter("data/user_list.txt", false);
            string json = JsonWriter.Serialize(infoList);
            writer.Write(json);
            writer.Close();
        }

        public bool IsUserDeviceId(string deviceId)
        {
            return infoList.userDeviceId.ContainsKey(deviceId);
        }

        public UserInfo GetUserDeviceId(string deviceId)
        {
            if (infoList.userDeviceId.ContainsKey(deviceId))
                return infoList.userDeviceId[deviceId];
            return null;
        }

        public User CreateUserDeviceId(string deviceId, NetConnection address)
        {
            uint id = CreateUserId();
            UserInfo info = new UserInfo() {DeviceId = deviceId, Id = id};
            infoList.AddInfo(info);

            User user = new User();
            user.id = id;
            user.deviceId = deviceId;
            user.units.Add(new Unit("lt1", 0));
            SaveUser(user);
            Data.t.dataBaseToGame.Add(new MsgGameLoadUser(user.id, user, address));
            return user;
        }

        private uint CreateUserId()
        {
            uint id = 1;
            while (infoList.users.ContainsKey(id))
            {
                id++;
            }
            return id;
        }

        public void SaveUser(User user)
        {
            string path = "data/users/" + user.id + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            StreamWriter writer = new StreamWriter(path + "user.txt");
            writer.Write(JsonWriter.Serialize(user));
            writer.Close();
        }

        public User LoadUserDeviceId(string deviceId)
        {
            UserInfo info = GetUserDeviceId(deviceId);
            if (info == null)
            {
                Tools.Log("logs/error_data_base.txt", "error load user: deviceId=" + deviceId);
                return null;
            }

            string path = "data/users/" + info.Id + "/";
            StreamReader reader = new StreamReader(path + "user.txt");
            string json = reader.ReadToEnd();
            reader.Close();
            User user = JsonReader.Deserialize<User>(json);
            return user;
        }
    }
}
