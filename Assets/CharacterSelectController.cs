using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectController : MonoBehaviour
{
    [SerializeField] List<GameObject> characters;

    int tempCharacter = 0;
    int currentCharacter = 0;

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

    public void Select()
    {
        currentCharacter = tempCharacter;
    }
}
