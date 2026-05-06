using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Itens")]
    public int coins = 0;
    public bool hasSword = false;

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

    public void AddSword()
    {
        hasSword = true;
        Debug.Log("Espada adquirida!");
    }
}