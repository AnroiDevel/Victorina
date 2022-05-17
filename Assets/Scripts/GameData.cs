using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace Victorina
{
    [System.Serializable]
    public class GameData
    {
        public static GameData Instance { get; private set; }
    }
}