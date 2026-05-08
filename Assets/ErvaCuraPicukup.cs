using UnityEngine;
using System.Collections;

public class ErvaCuraPickup : MonoBehaviour
{
    public int value = 1;
    private bool playerProximo = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerProximo = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerProximo = false;
    }

    private void Update()
    {
        if (playerProximo && Input.GetKeyDown(KeyCode.E))
        {
            ColetarErva();
        }
    }

    private void ColetarErva()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddErvaCura(value);
        Destroy(gameObject);
    }
}