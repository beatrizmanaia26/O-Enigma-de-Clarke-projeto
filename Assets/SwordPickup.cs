using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    [Header("Dica de interação")]
    public GameObject pickupHint;

    private bool playerPerto = false;
    private PlayerVisualSwap playerVisualSwap;

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
        if (playerVisualSwap != null)
        {
            playerVisualSwap.GetSword();

            if (pickupHint != null)
                pickupHint.SetActive(false);

            Debug.Log("Pegou a espada!");

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("PlayerVisualSwap não encontrado no Player.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;

            playerVisualSwap = other.GetComponent<PlayerVisualSwap>();

            if (playerVisualSwap == null)
                playerVisualSwap = other.GetComponentInParent<PlayerVisualSwap>();

            if (pickupHint != null)
                pickupHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
            playerVisualSwap = null;

            if (pickupHint != null)
                pickupHint.SetActive(false);
        }
    }
}