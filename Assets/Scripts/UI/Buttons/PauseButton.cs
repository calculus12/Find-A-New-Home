using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour, ButtonBase
{
    [SerializeField] GameObject pausePanel;
    public void OnClicked()
    {
        SoundManager.instance.PlayClickSound();
        GameManager.Instance.TogglePause();
        pausePanel.SetActive(true);
    }
}
