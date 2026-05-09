using UnityEngine;
using System.Collections;

public class ErvaCuraPickup : MonoBehaviour
{
    public int value = 1;
    public string idItem = ""; // Adicione um ID único (ex: "erva_fase3_1")
    private bool playerProximo = false;

    void Start()
    {
        // Se não tiver ID, cria um baseado no nome e posição
        if (string.IsNullOrEmpty(idItem))
            idItem = $"erva_{gameObject.name}_{transform.position.x}_{transform.position.y}";
    }

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
        // Registrar no SistemaProgresso
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SistemaProgresso progresso = player.GetComponent<SistemaProgresso>();
            if (progresso != null)
                progresso.ColetarItem(idItem);
        }
        
        // Adicionar ao inventário
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddErvaCura(value);
        
        Destroy(gameObject);
    }
}