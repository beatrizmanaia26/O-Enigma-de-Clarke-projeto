using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public float forcaPulo = 7f;
    
    [Header("Vidas")]
    public GameObject vida5;
    public GameObject vida4;
    public GameObject vida3;
    public GameObject vida2;
    public GameObject vida1;
    
    [Header("Invencibilidade")]
    public float tempoInvencibilidade = 1f;
    
    [Header("Derrota")]
    public string nomeCenaDerrota = "TelaDerrota";
    
    [Header("Limites da Tela")]
    public Transform limiteEsquerda;
    public Transform limiteDireita;
    
    [Header("Escada (teletransporte)")]
    // Não precisa mais de velocidadeEscada (sem movimento contínuo)
    private bool pertoDaEscada = false;
    private Ladder escadaDestino;   // referência ao script da escada
    
    private Rigidbody2D rb;
    private bool estaNoChao;
    private int vidasRestantes = 5;
    private bool invencivel = false;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        AtualizarVidas();
    }
    
    void Update()
    {
        // ========== MOVIMENTO NORMAL ==========
        float movimento = 0;
        if (Input.GetKey(KeyCode.D)) movimento = 1;
        else if (Input.GetKey(KeyCode.A)) movimento = -1;
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
        
        AplicarLimites();
        
        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
        
        // ========== USAR ESCADA (teletransporte) ==========
        if (Input.GetKeyDown(KeyCode.E) && pertoDaEscada && escadaDestino != null)
        {
            UsarEscada();
        }
    }
    
    void AplicarLimites()
    {
        if (limiteEsquerda == null || limiteDireita == null) return;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteEsquerda.position.x, limiteDireita.position.x);
        transform.position = pos;
    }
    
    // ========== DETECÇÃO DA ESCADA ==========
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("escada"))
        {
            pertoDaEscada = true;
            escadaDestino = other.GetComponent<Ladder>();
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("escada"))
        {
            pertoDaEscada = false;
            escadaDestino = null;
        }
    }
    
    void UsarEscada()
    {
        if (escadaDestino == null) return;
        
        // Se a escada tem uma cena destino, carrega a cena
        if (!string.IsNullOrEmpty(escadaDestino.nomeCenaDestino))
        {
            SceneManager.LoadScene(escadaDestino.nomeCenaDestino);
        }
        // Senão, teletransporta para o ponto de destino
        else if (escadaDestino.pontoDestino != null)
        {
            transform.position = escadaDestino.pontoDestino.position;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 1; // garante que a gravidade volte ao normal
            Debug.Log($"Teletransportado via escada: {escadaDestino.escadaID}");
        }
        else
        {
            Debug.LogWarning("Escada sem destino configurado!");
        }
    }
    
    // ========== COLISÕES COM INIMIGOS ==========
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("aranha"))
        {
            if (EstaEmCimaDoInimigo(collision))
            {
                MatarAranha(collision.gameObject);
                Debug.Log("Matou a aranha!");
            }
            else
            {
                if (!invencivel)
                {
                    PerderVida();
                    StartCoroutine(AtivarInvencibilidade());
                    Debug.Log("Encostou na aranha! Perdeu vida!");
                }
            }
        }
        else if (collision.gameObject.CompareTag("inimigo") && !invencivel)
        {
            PerderVida();
            StartCoroutine(AtivarInvencibilidade());
            Debug.Log("Encostou no inimigo! Perdeu vida!");
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao")) estaNoChao = true;
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao")) estaNoChao = false;
    }
    
    bool EstaEmCimaDoInimigo(Collision2D collision)
    {
        float inimigoTopo = collision.collider.bounds.max.y;
        float playerBase = GetComponent<Collider2D>().bounds.min.y;
        return playerBase >= inimigoTopo - 0.1f;
    }
    
    void MatarAranha(GameObject aranha)
    {
        Aranha scriptAranha = aranha.GetComponent<Aranha>();
        if (scriptAranha != null)
            scriptAranha.ReceberDano();
        else
            Destroy(aranha);
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo * 0.5f);
    }
    
    // ========== SISTEMA DE VIDA ==========
    void PerderVida()
    {
        if (vidasRestantes <= 0) return;
        vidasRestantes--;
        AtualizarVidas();
        Debug.Log("Perdeu vida! Vidas restantes: " + vidasRestantes);
        if (vidasRestantes <= 0) Morrer();
    }
    
    IEnumerator AtivarInvencibilidade()
    {
        invencivel = true;
        float t = 0;
        while (t < tempoInvencibilidade)
        {
            if (spriteRenderer) spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }
        if (spriteRenderer) spriteRenderer.enabled = true;
        invencivel = false;
    }
    
    void AtualizarVidas()
    {
        if (vida5) vida5.SetActive(vidasRestantes >= 5);
        if (vida4) vida4.SetActive(vidasRestantes >= 4);
        if (vida3) vida3.SetActive(vidasRestantes >= 3);
        if (vida2) vida2.SetActive(vidasRestantes >= 2);
        if (vida1) vida1.SetActive(vidasRestantes >= 1);
    }
    
    void Morrer()
    {
        Debug.Log("Player morreu! Carregando tela de derrota...");
        SceneManager.LoadScene(nomeCenaDerrota);
    }
    
    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}