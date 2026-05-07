using UnityEngine;
using TMPro;

public class InventoryUI_Manager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    
    [Header("Estrela")]
    public GameObject starIcon;
    public TextMeshProUGUI starCountText;
    
    [Header("Pedra Azul")]
    public GameObject blueStoneIcon;
    public TextMeshProUGUI blueStoneCountText;
    
    [Header("Flor")]
    public GameObject florIcon;
    public TextMeshProUGUI florCountText;
    
    [Header("Coroa")]
    public GameObject crownIcon;
    public TextMeshProUGUI crownCountText;

    private void Start() { UpdateUI(); }
    private void Update() { UpdateUI(); }

    private void UpdateUI()
    {
        if (InventoryManager.Instance == null) return;

        if (coinsText != null)
            coinsText.text = "Moedas: " + InventoryManager.Instance.coins;

        if (starCountText != null)
            starCountText.text = "x" + InventoryManager.Instance.stars;
        if (starIcon != null)
            starIcon.SetActive(InventoryManager.Instance.stars > 0);

        if (blueStoneCountText != null)
            blueStoneCountText.text = "x" + InventoryManager.Instance.blueStones;
        if (blueStoneIcon != null)
            blueStoneIcon.SetActive(InventoryManager.Instance.blueStones > 0);

        if (florCountText != null)
            florCountText.text = "x" + InventoryManager.Instance.flor;
        if (florIcon != null)
            florIcon.SetActive(InventoryManager.Instance.flor > 0);

        if (crownCountText != null)
            crownCountText.text = "x" + InventoryManager.Instance.crowns;
        if (crownIcon != null)
            crownIcon.SetActive(InventoryManager.Instance.crowns > 0);

    }
}