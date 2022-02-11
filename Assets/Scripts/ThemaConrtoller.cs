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
            string nameThema;

            if (PlayerPrefs.HasKey("Thema"))
            {
                nameThema = PlayerPrefs.GetString("Thema");
            }
            else nameThema = "ClassicThema";

            foreach (Thema thema in allThems)
                if (thema.name == nameThema)
                    ActiveThema = thema;
        }
    }

}