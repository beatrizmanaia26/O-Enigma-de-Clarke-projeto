using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Itens")]
    public int coins = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // mantém entre cenas
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
}