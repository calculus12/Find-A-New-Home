using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private GameObject selectButton;
    [SerializeField] private GameObject purchaseButton;

    private readonly Dictionary<string, Dictionary<string, object>> characterInfo =
        new Dictionary<string, Dictionary<string, object>>
        {
            {
                "Penguin", 
                new Dictionary<string, object>
                {
                    {"name", "Penguin"},
                    {"price", 0},
                }
            },
            {
                
                "Walrus", 
                new Dictionary<string, object>
                {
                    {"name", "Walrus"},
                    {"price", 20000},
                }
            },
            {
                "SeaLion", 
                new Dictionary<string, object>
                {
                    {"name", "SeaLion"},
                    {"price", 30000},
                }
            },
            {
                "PolarBear", 
                new Dictionary<string, object>
                {
                    {"name", "PolarBear"},
                    {"price", 10000},
                }
            },
        };
    
    int tempCharacterIndex = 0;
    int currentCharacterIndex = 0;
    string currentCharacterName = "Penguin";

    public Dictionary<String, object> CurCharacter
    {
        get { return characterInfo[characters[tempCharacterIndex].name]; }
    }

    private void Awake()
    {

        Transform[] allChildren = GetComponentsInChildren<Transform>(true);

        int i = 0;
        while (i < transform.childCount) // 첫번째 depth의 자식들(캐릭터들)을 characters에 초기화
        {
            characters.Add(transform.GetChild(i).gameObject);
            i++;
        }
    }

    private void Start()
    {
        currentCharacterIndex = GameManager.Instance.characterIndex;
        tempCharacterIndex = GameManager.Instance.characterIndex;

        characters[currentCharacterIndex].SetActive(true);
    }

    /// <summary>
    /// Left Button과 Right Button 눌렀을 때 캐릭터 임시 변경
    /// 캐릭터를 소유하고 있지 않다면 선택 버튼이 비활성화 및 가격 표시
    /// </summary>
    /// <param name="isNext">Right Button을 눌렀을 때 true, Left Button을 눌렀을 때 false</param>
    public void ChangeCharacter(bool isNext)
    {
        characters[tempCharacterIndex].SetActive(false);

        if (isNext)
        {
            tempCharacterIndex = (tempCharacterIndex + 1) % characters.Count;
        }
        else
        {
            tempCharacterIndex = tempCharacterIndex == 0 ? characters.Count - 1 : tempCharacterIndex - 1;
        }

        characters[tempCharacterIndex].SetActive(true);
        
        // 캐릭터를 갖고 있지 않다면 선택 버튼 비활성화 및 구매 버튼 활성화
        if (!PrefsManager.Instance.GetCharacterOwn(characters[tempCharacterIndex].name))
        {
            selectButton.SetActive(false);
            purchaseButton.SetActive(true);
            purchaseButton.GetComponent<PurchaseButton>().SetInfo();
        }
        // 캐릭터를 갖고 있다면 선택 버튼 활성화 및 구매 버튼 비활성화
        else
        {
            selectButton.SetActive(true);
            purchaseButton.SetActive(false);
        }
    }

    /// <summary>
    /// 캐릭터 선택 버튼을 누르고 캐릭터 변경
    /// </summary>
    public void SetCharacter()
    {
        characters[tempCharacterIndex].SetActive(false);
        currentCharacterIndex = tempCharacterIndex;
        currentCharacterName = characters[currentCharacterIndex].name;

        // 게임 매니저에 캐릭터 이름을 저장해서 게임 플레이 씬으로 넘어갔을 때
        // 선택한 캐릭터를 활성화하게끔 함
        GameManager.Instance.characterIndex = currentCharacterIndex;
        GameManager.Instance.characterName = currentCharacterName;

        // 현재 설정한 캐릭터 활성화
        characters[currentCharacterIndex].SetActive(true);
    }

    /// <summary>
    /// 캐릭터 선택하다 취소 버튼 눌렀을 때 원래 캐릭터로 되돌리기
    /// </summary>
    public void CancelChange()
    {
        characters[tempCharacterIndex].SetActive(false);
        characters[currentCharacterIndex].SetActive(true);
        tempCharacterIndex = currentCharacterIndex;
    }
}