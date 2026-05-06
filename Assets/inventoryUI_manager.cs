using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (InventoryManager.Instance != null && coinsText != null)
        {
            coinsText.text = "Moedas: " + InventoryManager.Instance.coins;
        }
    }
}