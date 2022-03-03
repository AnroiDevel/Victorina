using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public static class ThemaConrtoller
    {
        public static Thema ActiveThema;

        static ThemaConrtoller() { ThemaActivation(); }
        public static void ThemaActivation()
        {
            Resources.LoadAll<Thema>("");
            var allThems = Resources.FindObjectsOfTypeAll<Thema>();

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
    }

}