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
    public int crowns = 0;        // coroa antiga, deixa igual
    public int towerCrowns = 0;   // sua coroa nova da fase 7
    public int keys = 0;          // chave da fase 7
    public int torches = 0;       // tocha da fase 7
    public int ervaCura = 0;
    public bool hasSword = false;

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

    // Moedas
    public void AddCoin(int amount) 
    { 
        coins += amount; 
        Debug.Log("Moedas: " + coins); 
    }

    public bool SpendCoin(int amount) 
    { 
        if (coins >= amount) 
        { 
            coins -= amount; 
            return true; 
        } 
        return false; 
    }

    // Estrelas
    public void AddStar(int amount) 
    { 
        stars += amount; 
        Debug.Log("Estrelas: " + stars); 
    }

    public bool SpendStar(int amount) 
    { 
        if (stars >= amount) 
        { 
            stars -= amount; 
            return true; 
        } 
        return false; 
    }

    // Pedras Azuis
    public void AddBlueStone(int amount) 
    { 
        blueStones += amount; 
        Debug.Log("Pedras Azuis: " + blueStones); 
    }

    public bool SpendBlueStone(int amount) 
    { 
        if (blueStones >= amount) 
        { 
            blueStones -= amount; 
            return true; 
        } 
        return false; 
    }

    // ========== FLORES ==========
    public void AddFlor(int amount)
    {
        flor += amount;
        RegistrarOrdem("flor");
        Debug.Log("Flores: " + flor);
    }

    public bool SpendFlor(int amount) 
    { 
        if (flor >= amount) 
        { 
            flor -= amount; 
            return true; 
        } 
        return false; 
    }

    // ========== COROAS ANTIGAS ==========
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
            return true; 
        } 
        return false; 
    }

    // ========== SUA COROA NOVA DA FASE 7 ==========
    public void AddTowerCrown(int amount)
    {
        towerCrowns += amount;
        Debug.Log("Coroas da Torre: " + towerCrowns);
    }

    public bool SpendTowerCrown(int amount)
    {
        if (towerCrowns >= amount)
        {
            towerCrowns -= amount;
            Debug.Log($"Gastou {amount} coroa(s) da torre. Restam: {towerCrowns}");
            return true;
        }

        return false;
    }

    // ========== CHAVES ==========
    public void AddKey(int amount)
    {
        keys += amount;
        Debug.Log("Chaves: " + keys);
    }

    public bool SpendKey(int amount)
    {
        if (keys >= amount)
        {
            keys -= amount;
            Debug.Log($"Gastou {amount} chave(s). Restam: {keys}");
            return true;
        }

        return false;
    }

    // ========== TOCHAS ==========
    public void AddTorch(int amount)
    {
        torches += amount;
        Debug.Log("Tochas: " + torches);
    }

    public bool SpendTorch(int amount)
    {
        if (torches >= amount)
        {
            torches -= amount;
            Debug.Log($"Gastou {amount} tocha(s). Restam: {torches}");
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
    public void AddSword() 
    { 
        hasSword = true; 
        Debug.Log("Espada adquirida!"); 
    }

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