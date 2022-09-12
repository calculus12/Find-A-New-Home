using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingButton : MonoBehaviour, ButtonBase
{
    [SerializeField] GameObject mainScreen;
    public void OnClicked()
    {
        transform.parent.gameObject.SetActive(false);
        mainScreen.SetActive(true);
    }
}
