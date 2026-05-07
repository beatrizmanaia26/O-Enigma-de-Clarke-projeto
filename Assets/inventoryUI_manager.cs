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

    [Header("Erva de Cura")]          // ← NOVO
    public GameObject ervaCuraIcon;
    public TextMeshProUGUI ervaCuraCountText;

    [Header("Espada")]
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

        // Flores
        if (florCountText != null)
            florCountText.text = "x" + InventoryManager.Instance.flor;
        if (florIcon != null)
            florIcon.SetActive(InventoryManager.Instance.flor > 0);

        // Coroas
        if (crownCountText != null)
            crownCountText.text = "x" + InventoryManager.Instance.crowns;
        if (crownIcon != null)
            crownIcon.SetActive(InventoryManager.Instance.crowns > 0);

        // Erva de Cura (NOVO)
        if (ervaCuraCountText != null)
            ervaCuraCountText.text = "x" + InventoryManager.Instance.ervaCura;
        if (ervaCuraIcon != null)
            ervaCuraIcon.SetActive(InventoryManager.Instance.ervaCura > 0);

        // Espada
        if (swordIcon != null)
            swordIcon.SetActive(InventoryManager.Instance.hasSword);
    }
}