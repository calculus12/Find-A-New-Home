using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("UIManger instance is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// UI Canvas Ȱ��ȭ(��Ȱ��ȭ)
    /// </summary>
    /// <param name="go">Ȱ��ȭ(��Ȱ��ȭ)�� ĵ����</param>
    /// <param name="active">true�̸� Ȱ��ȭ false�̸� ��Ȱ��ȭ</param>
    public void SetCanvas(GameObject go, bool active)
    {
        go.SetActive(active);
    }
}
