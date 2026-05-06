using UnityEngine;

public class StarPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddStar(value);
            }
            Destroy(gameObject);
        }
    }
}