using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hi I'm " + gameObject.name);
        UIManager.Instance.SetCanvas(gameObject, false);
    }
}
