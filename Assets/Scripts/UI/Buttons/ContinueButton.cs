using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour, ButtonBase
{

    [SerializeField] GameObject pausePanel;

    public void OnClicked()
    {
        SoundManager.instance.PlayClickSound();
        GameManager.Instance.TogglePause();
        pausePanel.SetActive(false);
    }
}
