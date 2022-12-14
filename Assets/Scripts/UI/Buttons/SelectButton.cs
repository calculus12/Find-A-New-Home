using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour, ButtonBase
{
    [SerializeField] CharacterSelectController characterSelectController;
    [SerializeField] GameObject characterScreen;

    public void OnClicked()
    {
        SoundManager.instance.PlayClickSound();
        characterSelectController.SetCharacter();
        characterScreen.SetActive(false);
    }
}
