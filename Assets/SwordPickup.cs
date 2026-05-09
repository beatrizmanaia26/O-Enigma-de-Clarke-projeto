using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    public GameObject pickupHint;

    private bool playerPerto = false;

    private void Start()
    {
        if (pickupHint != null)
            pickupHint.SetActive(false);
    }

    private void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            PegarEspada();
        }
    }

    private void PegarEspada()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddSword();
        }
        else
        {
            Debug.LogWarning("InventoryManager não encontrado na cena.");
        }

        if (pickupHint != null)
            pickupHint.SetActive(false);

        Debug.Log("Pegou a espada!");

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;

            if (pickupHint != null)
                pickupHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;

            if (pickupHint != null)
                pickupHint.SetActive(false);
        }
    }
}