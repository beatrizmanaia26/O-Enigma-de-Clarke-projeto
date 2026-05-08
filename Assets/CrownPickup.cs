using UnityEngine;

public class CrownPickup : MonoBehaviour
{
    public GameObject pickupHint;
    public int value = 1;
    
    private bool playerProximo = false;

    private void Start()
    {
        if (pickupHint != null)
            pickupHint.SetActive(false);
        
        // Verifica se o InventoryManager existe via Instance
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance é NULL! Certifique-se que o InventoryManager existe na cena.");
        }
        else
        {
            Debug.Log("InventoryManager encontrado via Instance!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entrou no trigger da coroa: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerProximo = true;
            Debug.Log("Player perto da coroa! Aperte E para coletar");
            
            if (pickupHint != null)
                pickupHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = false;
            
            if (pickupHint != null)
                pickupHint.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerProximo && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Tecla E pressionada!");
            ColetarCoroa();
        }
    }
    
    private void ColetarCoroa()
    {
        Debug.Log("ColetarCoroa chamado!");
        
        // Usa o Instance diretamente
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddCrown(value);
            Debug.Log($"Coroa coletada! Total de coroas: {InventoryManager.Instance.crowns}");
            
            // Desbloqueia o player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerFase4 playerScript = player.GetComponent<PlayerFase4>();
                if (playerScript != null)
                {
                    playerScript.Desbloquear();
                }
            }
            
            if (pickupHint != null)
                pickupHint.SetActive(false);
            
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("ERRO: InventoryManager.Instance é NULL! Certifique-se que existe um GameObject com o script InventoryManager na cena.");
            
            // Tenta criar um InventoryManager automaticamente
            GameObject obj = new GameObject("InventoryManager");
            InventoryManager inv = obj.AddComponent<InventoryManager>();
            Debug.Log("InventoryManager criado automaticamente!");
            
            inv.AddCrown(value);
            Debug.Log($"Coroa coletada! Total: {inv.crowns}");
            Destroy(gameObject);
        }
    }
}