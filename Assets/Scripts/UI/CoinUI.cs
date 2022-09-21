using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    TextMeshProUGUI coin;
    private void Start()
    {
        coin = GetComponent<TextMeshProUGUI>();
        coin.text = PrefsManager.Instance.GetCoin().ToString();
    }

    public void ChangeText(int value)
    {
        coin.text = value.ToString();
    }
}
