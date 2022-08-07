using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayButton : MonoBehaviour, ButtonBase
{
    public void OnClicked()
    {
        SceneManager.LoadScene(1);
    }
}
