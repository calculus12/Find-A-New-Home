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
    /// ���� ����
    /// </summary>
    private GameState state = GameState.start;

    public GameState GameState
    {
        get { return state; }
    }

    /// <summary>
    /// SetState�� ����ɶ� ����� event
    /// </summary>
    public static event Action<GameState> OnGameStateChanged;

    /// <summary>
    /// ���� �Ŵ����� ���¸� �����Ѵ�.
    /// </summary>
    /// <param name="newState">�ٲ� ����<param>
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