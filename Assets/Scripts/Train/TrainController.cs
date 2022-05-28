using UnityEngine;


namespace Victorina
{
    public class TrainController : MonoBehaviour
    {
        private void Awake()
        {
            GameData.GetInstance().Player.IsTrain = true;
        }
    }
}