using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 로컬 데이터들을 불러오고 저장할 수 있는 컴포넌트, Singleton으로 구현되어 instance로 접근 가능
/// </summary>
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

    /// <summary>
    /// 로컬 데이터에 저장된 보유코인을 불러오는 함수
    /// </summary>
    /// <returns>보유코인을 반환, 만약 데이터가 없으면 0을 반환</returns>
    public int GetCoin()
    {
        return PlayerPrefs.GetInt("coin", 0);
    }

    /// <summary>
    /// 현재 코인을 로컬 데이터에 설정하는 함수
    /// </summary>
    /// <param name="coin">설정할 데이터 코인, 0 미만이면 설정하지 않음</param>
    public void SetCoin(int coin)
    {
        if (coin < 0) return;
        PlayerPrefs.SetInt("coin", coin);
    }

    /// <summary>
    /// 로컬 데이터에서 최고기록을 불러오는 함수
    /// </summary>
    /// <returns>최고기록을 반환, 만약 데이터가 없으면 0을 반환</returns>
    public int GetBestScore()
    {
        return PlayerPrefs.GetInt("best_score", 0);
    }

    /// <summary>
    /// 최고기록을 로컬 데이터에 설정하는 함수
    /// </summary>
    /// <param name="bestScore">설정할 최고기록, 0 미만이면 설정하지 않음</param>
    public void SetBestScore(int bestScore) 
    {
        if (bestScore < 0) return;
        PlayerPrefs.SetInt("best_score", bestScore);
    }

}
