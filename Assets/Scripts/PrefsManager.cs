using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsManager : MonoBehaviour
{
    private static PrefsManager _instance;

    public static PrefsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Prefs Manger instance is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int GetCoin()
    {
        return PlayerPrefs.GetInt("coin", 0);
    }

    public void SetCoin(int coin)
    {
        if (coin < 0) return;
        PlayerPrefs.SetInt("coin", coin);
    }

    public int GetBestScore()
    {
        return PlayerPrefs.GetInt("best_score", 0);
    }

    public void SetBestScore(int bestScore) 
    {
        if (bestScore < 0) return;
        PlayerPrefs.SetInt("best_score", bestScore);
    }

}
