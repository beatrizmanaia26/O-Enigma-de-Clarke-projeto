using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    [Header("Dica de interação")]
    public GameObject pickupHint;

    [Header("Save")]
    public string idItem = "espada_fase1";

    private bool playerPerto = false;
    private PlayerVisualSwap playerVisualSwap;

    private void Start()
    {
        if (pickupHint != null)
            pickupHint.SetActive(false);

        if (string.IsNullOrEmpty(idItem))
            idItem = "espada_fase1";
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
        // Troca visual / adiciona espada no inventário
        if (playerVisualSwap != null)
        {
            playerVisualSwap.GetSword();
        }
        else
        {
            Debug.LogWarning("[SwordPickup] PlayerVisualSwap não encontrado.");
        }

        // Registra no SistemaProgresso
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            SistemaProgresso progresso = player.GetComponent<SistemaProgresso>();

            if (progresso != null)
            {
                progresso.ColetarItem(idItem);
                Debug.Log("[SwordPickup] Espada registrada no progresso: " + idItem);
            }
            else
            {
                Debug.LogError("[SwordPickup] SistemaProgresso não encontrado no Player.");
            }
        }
        else
        {
            Debug.LogError("[SwordPickup] Player não encontrado pela tag Player.");
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