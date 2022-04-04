using UnityEngine;


public class Options : MonoBehaviour
{
    public void SendMail()
    {
        Application.OpenURL("mailto:anroidevel@gmail.com");
    }
}
