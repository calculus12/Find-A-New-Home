using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    private List<GameObject> characters = new List<GameObject>();

    int tempCharacter = 0;
    int currentCharacter = 0;

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
        currentCharacter = GameManager.Instance.character;
        tempCharacter = GameManager.Instance.character;

        characters[currentCharacter].SetActive(true);
    }

    /// <summary>
    /// Left Button과 Right Button 눌렀을 때 캐릭터 임시 변경
    /// </summary>
    /// <param name="isNext">Righut Button을 눌렀을 때 true</param>
    public void ChangeCharacter(bool isNext)
    {
        characters[tempCharacter].SetActive(false);

        if (isNext)
        {
            tempCharacter = (tempCharacter + 1) % characters.Count;
        }
        else
        {
            tempCharacter = tempCharacter == 0 ? characters.Count - 1 : tempCharacter - 1;
        }

        characters[tempCharacter].SetActive(true);
    }

    /// <summary>
    /// 캐릭터 선택 버튼을 누르고 캐릭터 변경
    /// </summary>
    public void SetCharacter()
    {
        characters[tempCharacter].SetActive(false);
        currentCharacter = tempCharacter;
        GameManager.Instance.character = currentCharacter;
        characters[currentCharacter].SetActive(true);
    }

    /// <summary>
    /// 캐릭터 선택하다 취소 버튼 눌렀을 때 원래 캐릭터로 되돌리기
    /// </summary>
    public void CancelChange()
    {
        characters[tempCharacter].SetActive(false);
        characters[currentCharacter].SetActive(true);
        tempCharacter = currentCharacter;
    }
}
