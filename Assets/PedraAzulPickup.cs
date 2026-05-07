using UnityEngine;

public class PedraAzulPickup : MonoBehaviour
{
    public int value = 1;
    private bool playerProximo = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = false;
        }
    }

    private void Update()
    {
        if (playerProximo && Input.GetKeyDown(KeyCode.E))
        {
            ColetarPedra();
        }
    }

    private void ColetarPedra()
    {
//         if (InventoryManager.Instance != null)
//         {
//             InventoryManager.Instance.AddBlueStone(value);
//         }
//         Destroy(gameObject);
    }
}