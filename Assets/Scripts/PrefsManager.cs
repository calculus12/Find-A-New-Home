using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����͵��� �ҷ����� ������ �� �ִ� ������Ʈ, Singleton���� �����Ǿ� instance�� ���� ����
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
    /// ���� �����Ϳ� ����� ���������� �ҷ����� �Լ�
    /// </summary>
    /// <returns>���������� ��ȯ, ���� �����Ͱ� ������ 0�� ��ȯ</returns>
    public int GetCoin()
    {
        return PlayerPrefs.GetInt("coin", 0);
    }

    /// <summary>
    /// ���� ������ ���� �����Ϳ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="coin">������ ������ ����, 0 �̸��̸� �������� ����</param>
    public void SetCoin(int coin)
    {
        if (coin < 0) return;
        PlayerPrefs.SetInt("coin", coin);
    }

    /// <summary>
    /// ���� �����Ϳ��� �ְ����� �ҷ����� �Լ�
    /// </summary>
    /// <returns>�ְ����� ��ȯ, ���� �����Ͱ� ������ 0�� ��ȯ</returns>
    public int GetBestScore()
    {
        return PlayerPrefs.GetInt("best_score", 0);
    }

    /// <summary>
    /// �ְ����� ���� �����Ϳ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="bestScore">������ �ְ���, 0 �̸��̸� �������� ����</param>
    public void SetBestScore(int bestScore) 
    {
        if (bestScore < 0) return;
        PlayerPrefs.SetInt("best_score", bestScore);
    }

}
