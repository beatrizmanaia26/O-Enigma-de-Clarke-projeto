using UnityEngine;
using System.Collections;

public class BruxaEnemy : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 3f;
    public Transform limiteEsquerda;
    public Transform limiteDireita;

    [Header("Vida")]
    public int health = 5;

    [Header("Save System")]
    public string idInimigo = "";

    private bool andandoParaDireita = true;
    private float limiteEsqX;
    private float limiteDirX;

    private SpriteRenderer spriteRenderer;
    private bool morreu = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (string.IsNullOrEmpty(idInimigo))
            idInimigo = $"bruxa_{gameObject.name}_{transform.position.x}_{transform.position.y}";

        ConfigurarLimites();

        Debug.Log("Bruxa iniciou com vida: " + health);
    }

    void Update()
    {
        if (!morreu)
            Patrulhar();
    }

    void ConfigurarLimites()
    {
        if (limiteEsquerda == null)
        {
            GameObject esq = GameObject.Find("limiteBruxa_esq");
            if (esq != null)
                limiteEsquerda = esq.transform;
        }

        if (limiteDireita == null)
        {
            GameObject dir = GameObject.Find("limiteBruxa_dir");
            if (dir != null)
                limiteDireita = dir.transform;
        }

        if (limiteEsquerda == null || limiteDireita == null)
        {
            limiteEsqX = transform.position.x - 3f;
            limiteDirX = transform.position.x + 3f;
        }
        else
        {
            limiteEsqX = limiteEsquerda.position.x;
            limiteDirX = limiteDireita.position.x;
        }
    }

    void Patrulhar()
    {
        float direcao = andandoParaDireita ? 1f : -1f;
        float novoX = transform.position.x + direcao * speed * Time.deltaTime;

        if (novoX >= limiteDirX)
        {
            novoX = limiteDirX;
            andandoParaDireita = false;
            Virar();
        }
        else if (novoX <= limiteEsqX)
        {
            novoX = limiteEsqX;
            andandoParaDireita = true;
            Virar();
        }

        transform.position = new Vector3(novoX, transform.position.y, transform.position.z);
    }

    void Virar()
    {
        Vector3 escala = transform.localScale;
        escala.x = andandoParaDireita ? Mathf.Abs(escala.x) : -Mathf.Abs(escala.x);
        transform.localScale = escala;
    }

    public void TakeDamage(int damage)
    {
        if (morreu) return;

        health -= damage;

        Debug.Log("Bruxa recebeu dano: " + damage + " | Vida restante: " + health);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(PiscarDano());
        }
    }

    void Die()
    {
        if (morreu) return;

        morreu = true;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            SistemaProgresso progresso = playerObj.GetComponent<SistemaProgresso>();

            if (progresso != null)
                progresso.DerrotarInimigo(idInimigo);
        }

        Debug.Log("Bruxa derrotada!");
        Destroy(gameObject);
    }

    IEnumerator PiscarDano()
    {
        if (spriteRenderer == null) yield break;

        Color original = spriteRenderer.color;

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = original;
    }
}