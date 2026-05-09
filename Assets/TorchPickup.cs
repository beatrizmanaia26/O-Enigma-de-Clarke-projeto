using UnityEngine;
using System.Collections;

public class TorchPickup : MonoBehaviour
{
    public int amount = 1;
    public string idItem = "";
    public float duracaoEfeito = 0.2f;
    public float escalaMaxima = 1.05f;

    private bool playerProximo = false;
    private bool coletando = false;
    private Vector3 escalaOriginal;
    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) corOriginal = spriteRenderer.color;
        escalaOriginal = transform.localScale;

        if (string.IsNullOrEmpty(idItem))
            idItem = $"tocha_{gameObject.name}_{transform.position.x}_{transform.position.y}";
        
        Debug.Log($"[TorchPickup] Start: idItem = {idItem}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !coletando)
            playerProximo = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerProximo = false;
    }

    void Update()
    {
        if (playerProximo && Input.GetKeyDown(KeyCode.E) && !coletando)
        {
            StartCoroutine(ColetarComEfeito());
        }
    }

    IEnumerator ColetarComEfeito()
    {
        coletando = true;
        playerProximo = false;

        float tempo = 0;
        while (tempo < duracaoEfeito)
        {
            float t = tempo / duracaoEfeito;
            float escalaAtual = Mathf.Lerp(1f, escalaMaxima, t);
            transform.localScale = escalaOriginal * escalaAtual;

            if (spriteRenderer != null)
            {
                Color corEfeito = Color.Lerp(corOriginal, new Color(1f, 0.5f, 0.2f), t);
                spriteRenderer.color = corEfeito;
            }

            tempo += Time.deltaTime;
            yield return null;
        }

        // Registra no sistema de progresso
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SistemaProgresso progresso = player.GetComponent<SistemaProgresso>();
            if (progresso != null)
            {
                progresso.ColetarItem(idItem);
                Debug.Log($"[TorchPickup] Coletado e registrado: {idItem}");
            }
            else
                Debug.LogError("[TorchPickup] SistemaProgresso não encontrado no Player!");
        }

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddTorch(amount);

        Destroy(gameObject);
    }
}