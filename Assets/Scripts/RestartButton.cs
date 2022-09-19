using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour, ButtonBase
{
    public void OnClicked()
    {
        SoundManager.instance.PlayClickSound();
        GameManager.Instance.RestartGame();
    }
}
