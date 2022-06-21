using UnityEngine;


namespace Victorina
{
    public static class ThemaConrtoller
    {
        #region Fields

        public static Thema ActiveThema;

        private static Thema[] _themas;

        #endregion

        static ThemaConrtoller() { ThemaActivation(); }

        #region Methods

        public static void ThemaActivation()
        {
            Resources.LoadAll<Thema>("");
            var allThems = Resources.FindObjectsOfTypeAll<Thema>();
            _themas = allThems;
            if (allThems.Length > 0)
            {
                GetActiveThema(allThems);
            }
        }

        private static void GetActiveThema(Thema[] allThems)
        {
            int nameThema;
            if (PlayerPrefs.HasKey("Thema"))
            {
                nameThema = PlayerPrefs.GetInt("Thema");
            }
            else nameThema = (int)ThemaType.ClassicThema;

            ThemaType themaType = 0;
            themaType += nameThema;

            foreach (Thema thema in allThems)
                if (thema.name == themaType.ToString())
                    ActiveThema = thema;
        }

        public static void SetActiveThema(string name)
        {
            foreach (var tema in _themas)
            {
                if (tema.name == name)
                    ActiveThema = tema;
            }
        }

        #endregion   
    }
}