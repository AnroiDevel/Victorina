using GoogleMobileAds.Api;
using UnityEngine;


public class AdMobInit : MonoBehaviour
{
    void Start()
    {
       MobileAds.Initialize(initStatus => { });
    }

}
