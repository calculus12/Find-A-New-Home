using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetting : MonoBehaviour
{
    string characterName = "Penguin";
    void Start()
    {
        // 캐릭터 이름을 전달받고 파괴시킨다
        characterName = GameManager.Instance.characterName;

        // 선택된 캐릭터를 활성화
        var character = transform.Find(characterName).gameObject;
        //Debug.Log(characterName);
        character.SetActive(true);
    }
}
