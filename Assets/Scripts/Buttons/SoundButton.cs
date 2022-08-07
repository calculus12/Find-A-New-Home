using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundButton : MonoBehaviour, ButtonBase
{
    [SerializeField] Image image;

    [SerializeField] Sprite soundOffImage;
    [SerializeField] Sprite soundOnImage;

    private bool isSoundOff = false;

    public void OnClicked()
    {
        if (isSoundOff)
        {
            image.sprite = soundOnImage;
            // 家府 难扁

            isSoundOff = false;
        }
        else
        {
            image.sprite = soundOffImage;

            // 家府 掺扁
            isSoundOff = true;
        }
    }
}
