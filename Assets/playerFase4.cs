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
    
    [Header("Limites")]
    public Transform limiteEsquerda;
    public Transform limiteDireita;
    
    [Header("Mensagens")]
    public string mensagemSemCoroa = "⚠️ PRECISA DE UMA COROA PARA PASSAR! ⚠️";
    
    private Rigidbody2D rb;
    private bool estaNoChao;
    private int vidasRestantes = 5;
    private bool invencivel = false;
    private SpriteRenderer spriteRenderer;
    private bool estaBloqueado = false;
    private float tempoMensagem = 0;
    
    // Variável para o baú da aranha (pode ser usada por outros scripts)
    public bool podePassarAranhaBau = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        AtualizarVidas();
        
        if (rb != null)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
    void Update()
    {
        // Se bloqueado, não pode andar
        if (estaBloqueado)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            
            if (Time.time > tempoMensagem)
            {
                Debug.LogWarning(mensagemSemCoroa);
                tempoMensagem = Time.time + 1f;
            }
            return;
        }
        
        // Movimento normal
        float movimento = 0;
        if (Input.GetKey(KeyCode.D)) movimento = 1;
        else if (Input.GetKey(KeyCode.A)) movimento = -1;
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
        
        // ========== VIRAR O CORPO NA DIREÇÃO DO MOVIMENTO ==========
        if (movimento != 0)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * (movimento > 0 ? 1 : -1);
            transform.localScale = escala;
        }
        
        AplicarLimites();
        
        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }
    
    void AplicarLimites()
    {
        if (limiteEsquerda == null || limiteDireita == null) return;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteEsquerda.position.x, limiteDireita.position.x);
        transform.position = pos;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = true;
        }
        
        if (collision.gameObject.CompareTag("inimigo") && !invencivel)
        {
            Debug.Log("Encostou no INIMIGO! Perdeu vida!");
            PerderVida();
            StartCoroutine(AtivarInvencibilidade());
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = true;
        }
        
        if (collision.gameObject.CompareTag("inimigo") && !invencivel)
        {
            PerderVida();
            StartCoroutine(AtivarInvencibilidade());
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = false;
        }
    }
    
    // Métodos públicos chamados por outros scripts (ex: baú)
    public void Desbloquear()
    {
        estaBloqueado = false;
        Debug.Log("PLAYER DESBLOQUEADO!");
    }
    
    public void Bloquear()
    {
        estaBloqueado = true;
        Debug.Log("PLAYER BLOQUEADO!");
    }
    
    void PerderVida()
    {
        if (vidasRestantes <= 0) return;
        vidasRestantes--;
        AtualizarVidas();
        Debug.Log("Perdeu vida! Vidas restantes: " + vidasRestantes);
        if (vidasRestantes <= 0) Morrer();
    }
    
    public void Morrer()
    {
        Debug.Log("Player morreu! Carregando tela de derrota...");
        SceneManager.LoadScene(nomeCenaDerrota);
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
    
    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // ========== MÉTODOS PARA SISTEMA DE SAVE ==========
    public int VidasRestantes 
    { 
        get { return vidasRestantes; }
        set 
        { 
            vidasRestantes = Mathf.Clamp(value, 0, 5); 
            AtualizarVidas();
        }
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}