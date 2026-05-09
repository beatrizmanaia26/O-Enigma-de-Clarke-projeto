using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour
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
            idItem = $"chave_{gameObject.name}_{transform.position.x}_{transform.position.y}";
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
                Color corEfeito = Color.Lerp(corOriginal, Color.yellow, t);
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
                progresso.ColetarItem(idItem);
        }

        // Adiciona ao inventário
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddKey(amount);

        Destroy(gameObject);
    }
}