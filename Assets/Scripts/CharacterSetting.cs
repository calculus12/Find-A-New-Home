using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetting : MonoBehaviour
{
    string characterName = "Penguin";
    void Start()
    {
        // ĳ���� �̸��� ���޹ް� �ı���Ų��
        characterName = GameManager.Instance.characterName;

        // ���õ� ĳ���͸� Ȱ��ȭ
        var character = transform.Find(characterName).gameObject;
        //Debug.Log(characterName);
        character.SetActive(true);
    }
}
