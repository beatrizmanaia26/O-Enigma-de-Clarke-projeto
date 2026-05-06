using UnityEngine;
using System.Collections;

public class Aranha : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidadeBase = 2f;
    public Transform limiteEsquerda;
    public Transform limiteDireita;

    private float velocidadeAtual;
    private bool movendoParaDireita = true;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int vida = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        velocidadeAtual = velocidadeBase;

        if (AranhaManager.Instance != null)
            AranhaManager.Instance.RegistrarAranha(this);
        else
            Debug.LogWarning("AranhaManager não encontrado na cena!");
    }

    void Update()
    {
        Mover();
    }

    void Mover()
    {
        if (limiteEsquerda == null || limiteDireita == null) return;
        float direcao = movendoParaDireita ? 1f : -1f;
        rb.linearVelocity = new Vector2(direcao * velocidadeAtual, rb.linearVelocity.y);

        if (movendoParaDireita && transform.position.x >= limiteDireita.position.x)
        {
            movendoParaDireita = false;
            Virar();
        }
        else if (!movendoParaDireita && transform.position.x <= limiteEsquerda.position.x)
        {
            movendoParaDireita = true;
            Virar();
        }
    }

    void Virar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void ReceberDano()
    {
        vida--;
        if (vida <= 0)
            Morrer();
        else
            StartCoroutine(Piscar());
    }

    void Morrer()
    {
        if (AranhaManager.Instance != null)
        {
            AranhaManager.Instance.RemoverAranha(this);
            AranhaManager.Instance.AoMatarAranha();
        }
        Destroy(gameObject);
    }

    public void AumentarVelocidade()
    {
        velocidadeAtual = velocidadeBase * 1.5f;
        if (sr != null)
            sr.color = new Color(1f, 0.5f, 0.5f); // avermelhado
        Debug.Log(name + " velocidade aumentada para " + velocidadeAtual);
    }

    public void TornarResistente()
    {
        vida = 2;
        if (sr != null)
            sr.color = new Color(0.5f, 0.5f, 1f); // azulado
        Debug.Log(name + " agora é resistente (vida = 2)");
    }

    IEnumerator Piscar()
    {
        if (sr == null) yield break;
        Color original = sr.color;
        for (int i = 0; i < 3; i++)
        {
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            sr.color = original;
            yield return new WaitForSeconds(0.1f);
        }
    }
}