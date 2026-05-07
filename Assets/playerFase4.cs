using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerFase4 : MonoBehaviour
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
        
        // Trava rotação para não tombar
        if (rb != null)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Garantir que a gravidade está ligada
        if (rb != null && rb.gravityScale == 0)
            rb.gravityScale = 1;
            
        Debug.Log("Player iniciado. Gravidade: " + rb.gravityScale);
    }
    
    void Update()
    {
        // ========== MOVIMENTO ==========
        float movimento = 0;
        if (Input.GetKey(KeyCode.D)) movimento = 1;
        else if (Input.GetKey(KeyCode.A)) movimento = -1;
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
        
        AplicarLimites();
        
        // ========== PULO ==========
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            Debug.Log("PULOU! Velocidade Y: " + rb.linearVelocity.y);
        }
    }
    
    void FixedUpdate()
    {
        // Verificação do chão por Raycast
        VerificarChaoPorRaycast();
    }
    
    void VerificarChaoPorRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);
        if (hit.collider != null && hit.collider.CompareTag("chao"))
        {
            estaNoChao = true;
        }
        else if (hit.collider == null)
        {
            estaNoChao = false;
        }
    }
    
    void AplicarLimites()
    {
        if (limiteEsquerda == null || limiteDireita == null) return;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteEsquerda.position.x, limiteDireita.position.x);
        transform.position = pos;
    }
    
    // ========== COLISÕES ==========
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica chão
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = true;
            Debug.Log("Player tocou no chão");
        }
        
        // Verifica aranha
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
        // Verifica outros inimigos
        else if (collision.gameObject.CompareTag("inimigo") && !invencivel)
        {
            PerderVida();
            StartCoroutine(AtivarInvencibilidade());
            Debug.Log("Encostou no inimigo! Perdeu vida!");
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = true;
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = false;
            Debug.Log("Player saiu do chão");
        }
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