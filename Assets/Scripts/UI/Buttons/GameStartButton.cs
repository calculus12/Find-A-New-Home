using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour, ButtonBase
{
    public void OnClicked()
    {
        SoundManager.instance.PlayClickSound();
        GameManager.Instance.StartGame();
    }
}
