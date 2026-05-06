using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public GameObject swordIcon;

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
        if (InventoryManager.Instance != null)
        {
            if (coinsText != null)
                coinsText.text = "Moedas: " + InventoryManager.Instance.coins;

            if (swordIcon != null)
                swordIcon.SetActive(InventoryManager.Instance.hasSword);
        }
    }
}