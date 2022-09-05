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
    /// UI Canvas 활성화(비활성화)
    /// </summary>
    /// <param name="go">활성화(비활성화)할 캔버스</param>
    /// <param name="active">true이면 활성화 false이면 비활성화</param>
    public void SetCanvas(GameObject go, bool active)
    {
        go.SetActive(active);
    }
}
