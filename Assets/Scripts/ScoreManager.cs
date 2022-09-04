using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    int score = 0;
    int earnedCoin = 0;

    public int GetScore
    {
        get { return score; }
    }

    public int GetEarnedCoin
    {
        get { return earnedCoin; }
    }

    [SerializeField] float scoreIncreaseInterval = 0.2f;

    float timePassed = 0f;

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.playing)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= scoreIncreaseInterval)
            {
                score += 1;
                scoreText.text = score.ToString();
                timePassed = 0f;
            }
        }
    }

    public void IncreaseCoin()
    {
        earnedCoin += 1;
    }
}
