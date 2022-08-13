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
        while (i < transform.childCount) // ù��° depth�� �ڽĵ�(ĳ���͵�)�� characters�� �ʱ�ȭ
        {
            characters.Add(transform.GetChild(i).gameObject);
            i++;
        }
    }

    private void Start()
    {
        currentCharacterIndex = GameManager.Instance.character;
        tempCharacter = GameManager.Instance.character;

        characters[currentCharacterIndex].SetActive(true);
    }

    /// <summary>
    /// Left Button�� Right Button ������ �� ĳ���� �ӽ� ����
    /// </summary>
    /// <param name="isNext">Righut Button�� ������ �� true</param>
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
    /// ĳ���� ���� ��ư�� ������ ĳ���� ����
    /// </summary>
    public void SetCharacter()
    {
        characters[tempCharacter].SetActive(false);
        currentCharacterIndex = tempCharacter;
        currentCharacterName = characters[currentCharacterIndex].name;

        // ���� �Ŵ����� ĳ���� �̸��� �����ؼ� ���� �÷��� ������ �Ѿ�� ��
        // ������ ĳ���͸� Ȱ��ȭ�ϰԲ� ��
        GameManager.Instance.characterName = currentCharacterName;

        // ���� ������ ĳ���� Ȱ��ȭ
        characters[currentCharacterIndex].SetActive(true);
    }

    /// <summary>
    /// ĳ���� �����ϴ� ��� ��ư ������ �� ���� ĳ���ͷ� �ǵ�����
    /// </summary>
    public void CancelChange()
    {
        characters[tempCharacter].SetActive(false);
        characters[currentCharacterIndex].SetActive(true);
        tempCharacter = currentCharacterIndex;
    }
}
