using UnityEngine;
using System.Collections;

public class FlorPickup : MonoBehaviour
{
    public int value = 1;
    public string idItem = "";
    public float duracaoBrilho = 0.5f;
    public float escalaBrilho = 1.2f;

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
        
        if (string.IsNullOrEmpty(idItem))
            idItem = $"flor_{gameObject.name}_{transform.position.x}_{transform.position.y}";
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

        float tempo = 0;
        while (tempo < duracaoBrilho)
        {
            float t = tempo / duracaoBrilho;
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

        // Registrar no SistemaProgresso
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SistemaProgresso progresso = player.GetComponent<SistemaProgresso>();
            if (progresso != null)
                progresso.ColetarItem(idItem);
        }

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddFlor(value);

        Destroy(gameObject);
    }
}