using UnityEngine;
using TMPro;

public class InitCoin : MonoBehaviour
{
    TextMeshProUGUI coin;
    private void Start()
    {
        coin = GetComponent<TextMeshProUGUI>();
        coin.text = PrefsManager.Instance.GetCoin().ToString();
    }
}
