using UnityEngine;
using System.Collections;

public class BruxaEnemy : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 3f;
    public Transform limiteEsquerda;
    public Transform limiteDireita;

    [Header("Vidas da Bruxa (Máximo 10)")]
    public GameObject vida10;
    public GameObject vida9;
    public GameObject vida8;
    public GameObject vida7;
    public GameObject vida6;
    public GameObject vida5;
    public GameObject vida4;
    public GameObject vida3;
    public GameObject vida2;
    public GameObject vida1;

    [Header("Save System")]
    public string idInimigo = "";

    private bool andandoParaDireita = true;
    private float limiteEsqX;
    private float limiteDirX;
    private SpriteRenderer spriteRenderer;
    private bool morreu = false;
    private int health = 10;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (string.IsNullOrEmpty(idInimigo))
            idInimigo = $"bruxa_{gameObject.name}_{transform.position.x}_{transform.position.y}";
        ConfigurarLimites();
        AtualizarVidasUI();
    }

    void Update()
    {
        if (!morreu) Patrulhar();
    }

    void ConfigurarLimites()
    {
        if (limiteEsquerda == null)
        {
            GameObject esq = GameObject.Find("limiteBruxa_esq");
            if (esq != null) limiteEsquerda = esq.transform;
        }
        if (limiteDireita == null)
        {
            GameObject dir = GameObject.Find("limiteBruxa_dir");
            if (dir != null) limiteDireita = dir.transform;
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
        if (health < 0) health = 0;

        AtualizarVidasUI();
        Debug.Log($"Bruxa levou {damage} de dano. Vida restante: {health}");

        if (health <= 0)
            Die();
        else
            StartCoroutine(PiscarDano());
    }

    void AtualizarVidasUI()
    {
        if (vida10 != null) vida10.SetActive(health >= 10);
        if (vida9 != null) vida9.SetActive(health >= 9);
        if (vida8 != null) vida8.SetActive(health >= 8);
        if (vida7 != null) vida7.SetActive(health >= 7);
        if (vida6 != null) vida6.SetActive(health >= 6);
        if (vida5 != null) vida5.SetActive(health >= 5);
        if (vida4 != null) vida4.SetActive(health >= 4);
        if (vida3 != null) vida3.SetActive(health >= 3);
        if (vida2 != null) vida2.SetActive(health >= 2);
        if (vida1 != null) vida1.SetActive(health >= 1);
    }

    void Die()
    {
        if (morreu) return;
        morreu = true;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            SistemaProgresso progresso = playerObj.GetComponent<SistemaProgresso>();
            if (progresso != null) progresso.DerrotarInimigo(idInimigo);
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