using UnityEngine;

public class PocaoPickup : MonoBehaviour
{
    public int value = 1;
    private bool playerProximo = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = true;
            Debug.Log("Player perto da poção. Aperte E para coletar.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = false;
            Debug.Log("Player saiu da área da poção.");
        }
    }

    private void Update()
    {
        if (playerProximo && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Tecla E pressionada. Coletando poção...");
            ColetarPocao();
        }
    }

    private void ColetarPocao()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddPocao(value);
            Debug.Log("Poção adicionada ao inventário!");
        }
        Destroy(gameObject);
    }
    void OnDestroy()
{
    Debug.Log($"Poção foi destruída! Causa rastreada: {StackTraceUtility.ExtractStackTrace()}");
}
}