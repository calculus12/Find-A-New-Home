using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameoverScreen : MonoBehaviour
{

    bool isBest = false;
    [SerializeField] TextMeshProUGUI currentScore;
    [SerializeField] TextMeshProUGUI previousBestScore;
    [SerializeField] TextMeshProUGUI earnedCoin;
    [SerializeField] GameObject isBestText;

    public void SetResult(int cScore, int eCoin)
    {
        currentScore.text = "Á¡¼ö: " + cScore.ToString();
        earnedCoin.text = eCoin.ToString();

        int pBest = PrefsManager.Instance.GetBestScore();

        previousBestScore.text = pBest.ToString();

        isBest = cScore > pBest;

        if (isBest)
        {
            isBestText.SetActive(true);
        }
        else
        {
            isBestText.SetActive(false);
        }
    }
}
