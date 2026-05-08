using UnityEngine;

public class CrownPickup_fase1 : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddCrown(amount);
                Destroy(gameObject);
            }
        }
    }
}