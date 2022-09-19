using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingButton : MonoBehaviour, ButtonBase
{
    [SerializeField] GameObject mainScreen;
    public void OnClicked()
    {
        SoundManager.instance.PlayClickSound();
        transform.parent.gameObject.SetActive(false);
        mainScreen.SetActive(true);
    }
}
