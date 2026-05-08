using UnityEngine;

public class TorchPickup : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.AddTorch(amount);
            Destroy(gameObject);
        }
    }
}