using UnityEngine;
using System.Collections;

public class PedraAzulPickup : MonoBehaviour
{
    public int value = 1;
    public float duracaoEfeito = 0.2f;
    public float escalaMaxima = 1.01f;   // aumenta apenas 1%

    private bool playerProximo = false;
    private bool coletando = false;
    private Vector3 escalaOriginal;
    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaOriginal = transform.localScale;
        if (spriteRenderer != null) corOriginal = spriteRenderer.color;
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
                Color corFinal = Color.Lerp(corOriginal, new Color(0.7f, 0.9f, 1f), t);
                spriteRenderer.color = corFinal;
            }

            tempo += Time.deltaTime;
            yield return null;
        }

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddBlueStone(value);
        Destroy(gameObject);
    }
}