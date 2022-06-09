using Google.Play.Review;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Victorina;


public class ReitingSendler : MonoBehaviour
{
    [SerializeField] private RateStarsSelector _rateStars;
    [SerializeField] private GameObject _reportPanel;
    [SerializeField] private GameObject _thanksPanel;
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;


    public void Review()
    {
        var mark = 0;
        if (PlayerPrefs.HasKey("MarkReview"))
        {
            mark = PlayerPrefs.GetInt("MarkReview");
            if (mark >= 4) return;
        }
        mark = _rateStars.UserGrade;
        GameData.GetInstance().Player.MarkApp = mark;
        PlayerPrefs.SetInt("MarkApp", mark);

        if (mark >= 4)
        {
            StartCoroutine(OpenReview());
        }
        else _reportPanel.SetActive(true);

    }

    private IEnumerator OpenReview()
    {
        _reviewManager = new ReviewManager();

        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        else
        {
            _thanksPanel.SetActive(true);
            PlayerPrefs.SetInt("MarkReview", _rateStars.UserGrade);
        }
    }

}
