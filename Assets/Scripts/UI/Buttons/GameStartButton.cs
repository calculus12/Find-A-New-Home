using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour, ButtonBase
{
    public void OnClicked()
    {
        GameManager.Instance.StartGame();
    }
}
