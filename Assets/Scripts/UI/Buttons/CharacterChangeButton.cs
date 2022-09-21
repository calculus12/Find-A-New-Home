﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterChangeButton : MonoBehaviour, ButtonBase
{
    [SerializeField] GameObject characterScreen;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] CharacterSelectController characterSelectController;
    [SerializeField] private GameObject characterSelectButton;
    [SerializeField] private GameObject characterPurchaseButton;

    private string textWhenSelectionInactive = "캐릭터 변경하기!";
    private string textWhenSelectionActive = "취소";
    public void OnClicked()
    {
        SoundManager.instance.PlayClickSound();
        // button 
        if (characterScreen.activeSelf)
        {
            characterSelectController.CancelChange();
            characterScreen.SetActive(false);
            changeTextWhenSelectionActive(false);
        }
        else
        {
            characterScreen.SetActive(true);
            characterSelectButton.SetActive(true);
            characterPurchaseButton.SetActive(false);
            changeTextWhenSelectionActive(true);
        }

    }
    public void changeTextWhenSelectionActive(bool active)
    {
        if (active)
        {
            buttonText.text = textWhenSelectionActive;
        }
        else
        {
            buttonText.text = textWhenSelectionInactive;
        }
    }
}
