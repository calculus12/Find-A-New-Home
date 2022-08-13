using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StopButton : MonoBehaviour, ButtonBase
{
    public void OnClicked()
    {
        GameManager.Instance.StopGame();
    }
}
