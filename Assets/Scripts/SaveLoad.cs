using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Victorina
{
    public static class SaveLoad
    {
        public static List<GameData> SavedGames = new List<GameData>();

        public static void Save()
        {
            SavedGames.Add(GameData.Instance);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
            bf.Serialize(file, SavedGames);
            file.Close();
        }


        public static void Load()
        {
            if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
                SavedGames = (List<GameData>)bf.Deserialize(file);
                file.Close();
            }
        }

    }


}