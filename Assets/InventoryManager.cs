using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;  // ← Singleton (obrigatório)
    
    [Header("Itens")]
    public int coins = 0;
    public int stars = 0;
    public int blueStones = 0;
    public int flor = 0;
    public int crowns = 0;
    public bool hasSword = false;

    private void Awake()
    {
        // Padrão Singleton - garante que só existe uma instância
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

    // Moedas
    public void AddCoin(int amount) { coins += amount; Debug.Log("Moedas: " + coins); }
    public bool SpendCoin(int amount) { if (coins >= amount) { coins -= amount; return true; } return false; }

    // Estrelas
    public void AddStar(int amount) { stars += amount; Debug.Log("Estrelas: " + stars); }
    public bool SpendStar(int amount) { if (stars >= amount) { stars -= amount; return true; } return false; }

    // Pedras Azuis
    public void AddBlueStone(int amount) { blueStones += amount; Debug.Log("Pedras Azuis: " + blueStones); }
    public bool SpendBlueStone(int amount) { if (blueStones >= amount) { blueStones -= amount; return true; } return false; }

    // Flor
    public void AddFlor(int amount) { flor += amount; Debug.Log("Flor: " + flor); }
    public bool SpendFlor(int amount) { if (flor >= amount) { flor -= amount; return true; } return false; }

    // Coroa
    public void AddCrown(int amount) { crowns += amount; Debug.Log("Coroas: " + crowns); }
    public bool SpendCrown(int amount) { if (crowns >= amount) { crowns -= amount; return true; } return false; }

    // Espada
    public void AddSword() { hasSword = true; Debug.Log("Espada adquirida!"); }
}