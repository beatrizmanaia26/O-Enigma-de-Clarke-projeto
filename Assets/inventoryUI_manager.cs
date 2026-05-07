using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    
    [Header("Estrela")]
    public GameObject starIcon;
    public TextMeshProUGUI starCountText;
    
    [Header("Pedra Azul")]
    public GameObject blueStoneIcon;
    public TextMeshProUGUI blueStoneCountText;
    
    [Header("Flor")]                    // ← NOVO
    public GameObject florIcon;
    public TextMeshProUGUI florCountText;
    
    public GameObject swordIcon;

    private void Start() { UpdateUI(); }
    private void Update() { UpdateUI(); }

    private void UpdateUI()
    {
        if (InventoryManager.Instance == null) return;

        // Moedas
        if (coinsText != null)
            coinsText.text = "Moedas: " + InventoryManager.Instance.coins;

        // Estrelas
        if (starCountText != null)
            starCountText.text = "x" + InventoryManager.Instance.stars;
        if (starIcon != null)
            starIcon.SetActive(InventoryManager.Instance.stars > 0);

        // Pedras Azuis
        if (blueStoneCountText != null)
            blueStoneCountText.text = "x" + InventoryManager.Instance.blueStones;
        if (blueStoneIcon != null)
            blueStoneIcon.SetActive(InventoryManager.Instance.blueStones > 0);

        // Flor (NOVO)
        if (florCountText != null)
            florCountText.text = "x" + InventoryManager.Instance.flor;
        if (florIcon != null)
            florIcon.SetActive(InventoryManager.Instance.flor > 0);

        // Espada
        if (swordIcon != null)
            swordIcon.SetActive(InventoryManager.Instance.hasSword);
    }
}