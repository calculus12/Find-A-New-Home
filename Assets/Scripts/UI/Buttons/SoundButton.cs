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
            // 소리 켜기

            isSoundOff = false;
        }
        else
        {
            image.sprite = soundOffImage;

            // 소리 끄기
            isSoundOff = true;
        }
    }
}
