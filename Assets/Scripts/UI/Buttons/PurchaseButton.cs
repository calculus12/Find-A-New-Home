using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseButton : MonoBehaviour, ButtonBase
{
    [SerializeField] private GameObject selectButton;
    [SerializeField] private CharacterSelectController controller;
    [SerializeField] private Sprite disableBackground;
    [SerializeField] private Sprite ableBackground;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private CoinUI coinText;
    

    private string characterName;
    private int price;

    public void SetInfo()
    {
        var info = controller.CurCharacter;
        price = (int)info["price"];
        characterName = (string)info["name"];

        background.sprite = price > PrefsManager.Instance.GetCoin() ? disableBackground : ableBackground;
        priceText.text = price.ToString();
    }

    public void OnClicked()
    {
        // var info = controller.CurCharacter;
        // int price = info["price"] ?? 99999;
        // string characterName = info["name"] ?? "";
        // 돈이 부족하면 구매 불가
        if (price > PrefsManager.Instance.GetCoin())
        {
            return;
        }
        // 돈이 있다면 구매처리
        else
        {
            PrefsManager.Instance.SetCoin(PrefsManager.Instance.GetCoin() - price);
            PrefsManager.Instance.SetCharacterOwn(characterName, true);
            // 선택 버튼 활성화
            selectButton.SetActive(true);
            gameObject.SetActive(false);
            // UI 코인 값 변경
            coinText.ChangeText(PrefsManager.Instance.GetCoin());
        }
    }
}
