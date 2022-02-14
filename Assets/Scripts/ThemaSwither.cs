using UnityEngine;


namespace Victorina
{
    public class ThemaSwither : MonoBehaviour
    {
        [SerializeField] private ProfileThema _profileThema;

        public void SwithThema()
        {
            ThemaType themaType = 0;
            if (PlayerPrefs.HasKey("Thema"))
            {
                themaType += PlayerPrefs.GetInt("Thema");
            }
            themaType++;

            if (themaType > ThemaType.NeonThema)
                themaType = ThemaType.ClassicThema;

            PlayerPrefs.SetInt("Thema", (int)themaType);
            ThemaConrtoller.ThemaActivation();
            _profileThema.ThemaApply();
        }
    }

    public enum ThemaType
    {
        ClassicThema,
        FuturismThema,
        NeonThema,
    }

}