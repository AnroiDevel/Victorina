using UnityEngine;


namespace Victorina
{
    public class ThemaSwither : MonoBehaviour
    {
        [SerializeField] private ProfileThema _profileThema;

        public void SwithThema(string nameThema)
        {
            if (PlayerPrefs.HasKey("Thema"))
            {
                var nt = PlayerPrefs.GetString("Thema");
                if (nt == nameThema)
                    nameThema = "ClassicThema";
            }

            PlayerPrefs.SetString("Thema", nameThema);
            ThemaConrtoller.ThemaActivation();
            _profileThema.ThemaApply();
        }
    }

}