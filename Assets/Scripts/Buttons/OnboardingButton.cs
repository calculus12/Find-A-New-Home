using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingButton : MonoBehaviour, ButtonBase
{

    [SerializeField] GameObject startScreen;
    public void OnClicked()
    {
        UIManager.Instance.SetCanvas(gameObject.transform.parent.gameObject, false);
        UIManager.Instance.SetCanvas(startScreen, true);
    }
}
