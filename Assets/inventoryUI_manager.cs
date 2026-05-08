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

    [Header("Coroa antiga")]
    public GameObject crownIcon;
    public TextMeshProUGUI crownCountText;

    [Header("Erva de Cura")]
    public GameObject ervaCuraIcon;
    public TextMeshProUGUI ervaCuraCountText;

    [Header("Espada")]
    public GameObject swordIcon;

    [Header("Chave Fase 7")]
    public GameObject keyIcon;
    public TextMeshProUGUI keyCountText;

    [Header("Tocha Fase 7")]
    public GameObject torchIcon;
    public TextMeshProUGUI torchCountText;

    [Header("Coroa da Torre Fase 7")]
    public GameObject towerCrownIcon;
    public TextMeshProUGUI towerCrownCountText;

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

        // Coroa antiga
        if (crownCountText != null)
            crownCountText.text = "x" + InventoryManager.Instance.crowns;

        if (crownIcon != null)
            crownIcon.SetActive(InventoryManager.Instance.crowns > 0);

        // Erva de Cura
        if (ervaCuraCountText != null)
            ervaCuraCountText.text = "x" + InventoryManager.Instance.ervaCura;

        if (ervaCuraIcon != null)
            ervaCuraIcon.SetActive(InventoryManager.Instance.ervaCura > 0);

        // Espada
        if (swordIcon != null)
            swordIcon.SetActive(InventoryManager.Instance.hasSword);

        // Chave fase 7
        if (keyCountText != null)
            keyCountText.text = "x" + InventoryManager.Instance.keys;

        if (keyIcon != null)
            keyIcon.SetActive(InventoryManager.Instance.keys > 0);

        // Tocha fase 7
        if (torchCountText != null)
            torchCountText.text = "x" + InventoryManager.Instance.torches;

        if (torchIcon != null)
            torchIcon.SetActive(InventoryManager.Instance.torches > 0);

        // Coroa da torre fase 7
        if (towerCrownCountText != null)
            towerCrownCountText.text = "x" + InventoryManager.Instance.towerCrowns;

        if (towerCrownIcon != null)
            towerCrownIcon.SetActive(InventoryManager.Instance.towerCrowns > 0);
    }
}