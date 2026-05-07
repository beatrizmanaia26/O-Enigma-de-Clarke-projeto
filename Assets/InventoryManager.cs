using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Itens")]
    public int coins = 0;
    public int stars = 0;
    public int blueStones = 0;
    public int flor = 0;
    public int crowns = 0;
    public int ervaCura = 0;      // ← NOVO
    public bool hasSword = false;

    // Controle da ordem de coleta (para a poção)
    public List<string> ordemColeta = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ========== MOEDAS ==========
    public void AddCoin(int amount) { coins += amount; Debug.Log("Moedas: " + coins); }
    public bool SpendCoin(int amount) { if (coins >= amount) { coins -= amount; return true; } return false; }

    // ========== ESTRELAS ==========
    public void AddStar(int amount)
    {
        stars += amount;
        RegistrarOrdem("estrela");
        Debug.Log("Estrelas: " + stars);
    }
    public bool SpendStar(int amount) { if (stars >= amount) { stars -= amount; return true; } return false; }

    // ========== PEDRAS AZUIS ==========
    public void AddBlueStone(int amount)
    {
        blueStones += amount;
        RegistrarOrdem("pedraAzul");
        Debug.Log("Pedras Azuis: " + blueStones);
    }
    public bool SpendBlueStone(int amount) { if (blueStones >= amount) { blueStones -= amount; return true; } return false; }

    // ========== FLORES ==========
    public void AddFlor(int amount)
    {
        flor += amount;
        RegistrarOrdem("flor");
        Debug.Log("Flores: " + flor);
    }
    public bool SpendFlor(int amount) { if (flor >= amount) { flor -= amount; return true; } return false; }

    // ========== COROAS ==========
    public void AddCrown(int amount)
    {
        crowns += amount;
        Debug.Log("Coroas: " + crowns);
    }
    public bool SpendCrown(int amount)
    {
        if (crowns >= amount)
        {
            crowns -= amount;
            Debug.Log($"Gastou {amount} coroa(s). Restam: {crowns}");
            return true;
        }
        return false;
    }

    // ========== ERVA DE CURA ==========
    public void AddErvaCura(int amount)
    {
        ervaCura += amount;
        Debug.Log("Erva de Cura: " + ervaCura);
    }
    public bool SpendErvaCura(int amount)
    {
        if (ervaCura >= amount)
        {
            ervaCura -= amount;
            return true;
        }
        return false;
    }

    // ========== ESPADA ==========
    public void AddSword() { hasSword = true; Debug.Log("Espada adquirida!"); }

    // ========== MÉTODOS DE ORDEM ==========
    private void RegistrarOrdem(string item)
    {
        if (!ordemColeta.Contains(item))
            ordemColeta.Add(item);
    }

    public void ResetarOrdem()
    {
        ordemColeta.Clear();
    }
}