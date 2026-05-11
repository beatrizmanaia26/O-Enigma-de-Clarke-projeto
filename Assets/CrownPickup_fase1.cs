using UnityEngine;

public class CrownPickup_fase1 : MonoBehaviour
{
    public int amount = 1;

    [Header("Save")]
    public string idItem = "coroa_fase1";

    private bool playerPerto = false;

    private void Start()
    {
        if (string.IsNullOrEmpty(idItem))
            idItem = "coroa_fase1";
    }

    private void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            PegarCoroa();
        }
    }

    private void PegarCoroa()
    {
        // Adiciona no inventário
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddCrown(amount);
            Debug.Log("Pegou a coroa!");
        }
        else
        {
            Debug.LogWarning("[CrownPickup_fase1] InventoryManager não encontrado.");
        }

        // Registra no SistemaProgresso
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            SistemaProgresso progresso = player.GetComponent<SistemaProgresso>();

            if (progresso != null)
            {
                progresso.ColetarItem(idItem);
                Debug.Log("[CrownPickup_fase1] Coroa registrada no progresso: " + idItem);
            }
            else
            {
                Debug.LogError("[CrownPickup_fase1] SistemaProgresso não encontrado no Player.");
            }
        }
        else
        {
            Debug.LogError("[CrownPickup_fase1] Player não encontrado pela tag Player.");
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