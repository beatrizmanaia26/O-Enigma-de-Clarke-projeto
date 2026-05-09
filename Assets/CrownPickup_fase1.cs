using UnityEngine;

public class CrownPickup_fase1 : MonoBehaviour
{
    public int amount = 1;

    private bool playerPerto = false;

    private void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            PegarCoroa();
        }
    }

    private void PegarCoroa()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddCrown(amount);
            Debug.Log("Pegou a coroa!");
        }
        else
        {
            Debug.LogWarning("InventoryManager não encontrado na cena.");
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPerto = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPerto = false;
        }
    }
}