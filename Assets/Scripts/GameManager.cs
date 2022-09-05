using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manger instance is null");
            }

            return _instance;
        }
    }

    public int characterIndex { get; set; } = 0;
    public string characterName { get; set; } = "Penguin";

    /// <summary>
    /// 현재 상태
    /// </summary>
    private GameState state = GameState.start;

    public GameState GameState
    {
        get { return state; }
    }

    /// <summary>
    /// SetState가 실행될때 실행될 event
    /// </summary>
    public static event Action<GameState> OnGameStateChanged;

    /// <summary>
    /// 게임 매니저의 상태를 설정한다.
    /// </summary>
    /// <param name="newState">바꿀 상태<param>
    public void SetState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.start:
                break;
            case GameState.playing:
                break;
            case GameState.pause:
                break;
            case GameState.gameover:
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }
    
    public void TogglePause()
    {
        if (state == GameState.pause)
        {
            SetState(GameState.playing);
            Time.timeScale = 1f;
            return;
        }

        SetState(GameState.pause);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        SetState(GameState.playing);
        SceneManager.LoadScene(1);
    }

    public void StopGame()
    {
        Time.timeScale = 1f;
        SetState(GameState.start);
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SetState(GameState.playing);
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 게임오버 처리 및 플레이어의 움직임을 비활성화 하는 함수
    /// OnGame Scene에서만 호출되어야 한다.
    /// </summary>
    /// <param name="curScore">현재 점수</param>
    /// <param name="earnedCoin">획득한 코인</param>
    public void GameoverAndSave(int curScore, int earnedCoin)
    {
        SetState(GameState.gameover);

        // player의 움직임 컴포넌트를 비활성화
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<Movement>().enabled = false;
        player.GetComponent<SwipeManager>().enabled = false;

        // 장애물 생성기 비활성화
        GameObject.Find("ObjectGenerator")?.SetActive(false);


        // 코인 설정
        int curCoin = PrefsManager.Instance.GetCoin();
        PrefsManager.Instance.SetCoin(curCoin + earnedCoin);

        // 최고기록 설정
        if (curScore > PrefsManager.Instance.GetBestScore())
        {
            PrefsManager.Instance.SetBestScore(curScore);
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

}

public enum GameState
{
    start,
    playing,
    pause,
    gameover,
}