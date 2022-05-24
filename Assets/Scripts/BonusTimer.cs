using System;
using System.Threading.Tasks;


namespace Victorina
{
    internal class BonusTimer
    {
        private const string Complite = "Готово";
        private static BonusTimer _instance;
        private bool _isActiveTimer;

        GameData _gameData;

        public BonusTimer()
        {
            _gameData = GameData.GetInstance();
            _isActiveTimer = false;
        }


        public string LeftTime { get; private set; }


        public static BonusTimer GetInstance()
        {
            return _instance ??= new BonusTimer();
        }


        public async void StartTimer(int timeSec)
        {
            if (_gameData.Player.Bonus <= 0 && !_isActiveTimer)
            {
                _isActiveTimer = true;
                while (timeSec-- > 0)
                {
                    LeftTime = DateTime.MinValue.AddSeconds(timeSec).ToLongTimeString();
                    await Task.Delay(1000);
                }
            }
            _isActiveTimer &= false;
            LeftTime = Complite;

        }
    }
}
