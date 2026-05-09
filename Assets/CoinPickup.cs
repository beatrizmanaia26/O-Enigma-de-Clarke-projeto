using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;
    public string idItem = "";

    void Start()
    {
        if (string.IsNullOrEmpty(idItem))
            idItem = $"moeda_{gameObject.name}_{transform.position.x}_{transform.position.y}";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SistemaProgresso progresso = other.GetComponent<SistemaProgresso>();
            if (progresso != null) progresso.ColetarItem(idItem);

            if (InventoryManager.Instance != null)
                InventoryManager.Instance.AddCoin(value);

            Destroy(gameObject);
        }
    }
}