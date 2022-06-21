using System;
using System.Threading.Tasks;


namespace Victorina
{
    internal class BonusTimer
    {
        #region Fields

        private const string Complite = "Готово";
        private static BonusTimer _instance;
        private bool _isActiveTimer;

        GameData _gameData;

        #endregion


        #region ClassLifeCycles
        public BonusTimer()
        {
            _gameData = GameData.GetInstance();
            _isActiveTimer = false;
        }
        public static BonusTimer GetInstance() => _instance ??= new BonusTimer();

        #endregion


        #region Properties

        public string LeftTime { get; private set; }

        #endregion


        #region Methods

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

        #endregion 
    }
}
