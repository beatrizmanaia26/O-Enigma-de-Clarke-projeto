using UnityEngine;
using System.Collections;

public class FlorPickup : MonoBehaviour
{
    public int value = 1;
    public float duracaoBrilho = 0.5f;  // tempo que brilha antes de sumir
    public float escalaBrilho = 1.2f;   // tamanho máximo do brilho

    private bool playerProximo = false;
    private bool coletando = false;
    private Vector3 escalaOriginal;
    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaOriginal = transform.localScale;
        if (spriteRenderer != null) corOriginal = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !coletando)
            playerProximo = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerProximo = false;
    }

    private void Update()
    {
        if (playerProximo && Input.GetKeyDown(KeyCode.E) && !coletando)
        {
            StartCoroutine(ColetarComBrilho());
        }
    }

    private IEnumerator ColetarComBrilho()
    {
        coletando = true;
        playerProximo = false;

        // Efeito de brilho (aumenta e muda cor)
        float tempo = 0;
        while (tempo < duracaoBrilho)
        {
            float t = tempo / duracaoBrilho; // 0 → 1
            float escala = Mathf.Lerp(1f, escalaBrilho, t);
            transform.localScale = escalaOriginal * escala;

            if (spriteRenderer != null)
            {
                Color corBrilho = Color.Lerp(corOriginal, Color.yellow, t);
                spriteRenderer.color = corBrilho;
            }

            tempo += Time.deltaTime;
            yield return null;
        }

        // Adiciona ao inventário
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddFlor(value);

        // Destroi a flor
        Destroy(gameObject);
    }
}