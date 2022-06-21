namespace Victorina
{
    [System.Serializable]
    public class GameData
    {
        private static GameData _instance;

        public Character Player;

        public static GameData GetInstance()
        {
            return _instance ??= new GameData();
        }

        public GameData()
        {
            Player = new Character();
        }
    }
}