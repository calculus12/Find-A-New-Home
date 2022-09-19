using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightButton : MonoBehaviour, ButtonBase
{
    [SerializeField] CharacterSelectController controller;
    public void OnClicked()
    {
        SoundManager.instance.PlayClickSound();
        controller.ChangeCharacter(true);
    }
}
