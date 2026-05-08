using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.AddKey(amount);
            Destroy(gameObject);
        }
    }
}