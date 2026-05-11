using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerFase4 : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public float runSpeed = 8f;          // Velocidade da corrida
    public float forcaPulo = 7f;

    [Header("Agachamento")]
    public float crouchSpeed = 2f;                // Velocidade quando agachado
    public GameObject clarkeAgachada;             // Sprite do personagem agachado
    public Vector2 colliderNormalSize;            // Tamanho do collider normal
    public Vector2 colliderNormalOffset;          // Offset do collider normal
    public Vector2 colliderAgachadoSize;          // Tamanho do collider agachado
    public Vector2 colliderAgachadoOffset;        // Offset do collider agachado

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
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private bool estaNoChao;
    private int vidasRestantes = 5;
    private bool invencivel = false;
    private bool estaBloqueado = false;
    private float tempoMensagem = 0;
    private bool isCrouching = false;      // Estado do agachamento

    // Variável para o baú da aranha
    public bool podePassarAranhaBau = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Salvar tamanhos normais do collider se não fornecidos
        if (boxCollider != null)
        {
            if (colliderNormalSize == Vector2.zero)
                colliderNormalSize = boxCollider.size;
            if (colliderNormalOffset == Vector2.zero)
                colliderNormalOffset = boxCollider.offset;
        }

        // Inicia com o visual agachado desativado
        if (clarkeAgachada != null)
            clarkeAgachada.SetActive(false);

        AtualizarVidas();

        if (rb != null)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Se bloqueado, não pode andar, correr, pular ou agachar
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

        // ========== AGACHAMENTO (TECLAS CTRL ou C) ==========
        if (Input.GetKeyDown(KeyCode.C) ||
            Input.GetKeyDown(KeyCode.LeftControl) ||
            Input.GetKeyDown(KeyCode.RightControl))
        {
            if (isCrouching)
                Levantar();
            else
                Agachar();
        }

        // ========== MOVIMENTO (com corrida e agachamento) ==========
        float movimento = 0;
        if (Input.GetKey(KeyCode.D)) movimento = 1;
        else if (Input.GetKey(KeyCode.A)) movimento = -1;

        float velocidadeAtual = velocidade;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            velocidadeAtual = runSpeed;
        if (isCrouching)
            velocidadeAtual = crouchSpeed;

        rb.linearVelocity = new Vector2(movimento * velocidadeAtual, rb.linearVelocity.y);

        // ========== VIRAR O CORPO NA DIREÇÃO DO MOVIMENTO ==========
        if (movimento != 0)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * (movimento > 0 ? 1 : -1);
            transform.localScale = escala;

            // Aplica flip também no visual agachado
            if (clarkeAgachada != null)
                AplicarFlip(clarkeAgachada);
        }

        AplicarLimites();

        // ========== PULO (proibido quando agachado) ==========
        if (!isCrouching && Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }

    void Agachar()
    {
        if (isCrouching) return;
        isCrouching = true;

        // Desabilita sprite normal e habilita sprite agachada
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        if (clarkeAgachada != null)
        {
            clarkeAgachada.SetActive(true);
            AplicarFlip(clarkeAgachada);
            clarkeAgachada.transform.localPosition = Vector3.zero; // alinha com o pivô
        }

        // Ajusta o collider para o modo agachado
        if (boxCollider != null)
        {
            boxCollider.size = colliderAgachadoSize;
            boxCollider.offset = colliderAgachadoOffset;
        }
    }

    void Levantar()
    {
        if (!isCrouching) return;
        isCrouching = false;

        // Reabilita sprite normal e desabilita sprite agachada
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        if (clarkeAgachada != null)
            clarkeAgachada.SetActive(false);

        // Restaura o collider normal
        if (boxCollider != null)
        {
            boxCollider.size = colliderNormalSize;
            boxCollider.offset = colliderNormalOffset;
        }
    }

    void AplicarFlip(GameObject obj)
    {
        if (obj == null) return;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = transform.localScale.x < 0;
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
            estaNoChao = true;

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
            estaNoChao = true;

        if (collision.gameObject.CompareTag("inimigo") && !invencivel)
        {
            PerderVida();
            StartCoroutine(AtivarInvencibilidade());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
            estaNoChao = false;
    }

    // Métodos públicos
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
        // Remove o piscar da sprite (versão sem efeito visual)
        yield return new WaitForSeconds(tempoInvencibilidade);
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