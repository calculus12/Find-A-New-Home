using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    private List<GameObject> characters = new List<GameObject>();

    int tempCharacter = 0;
    int currentCharacterIndex = 0;
    string currentCharacterName = "Penguin";

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
        tempCharacter = GameManager.Instance.characterIndex;

        characters[currentCharacterIndex].SetActive(true);
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
        currentCharacterIndex = tempCharacter;
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
        characters[tempCharacter].SetActive(false);
        characters[currentCharacterIndex].SetActive(true);
        tempCharacter = currentCharacterIndex;
    }
}
