using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerFase4 : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public float runSpeed = 8f;
    public float forcaPulo = 7f;

    [Header("Agachamento")]
    public float crouchSpeed = 2f;
    public GameObject clarkeAgachada;
    public Vector2 colliderNormalSize;
    public Vector2 colliderNormalOffset;
    public Vector2 colliderAgachadoSize;
    public Vector2 colliderAgachadoOffset;

    [Header("Vidas")]
    public GameObject vida5;
    public GameObject vida4;
    public GameObject vida3;
    public GameObject vida2;
    public GameObject vida1;

    [Header("Invencibilidade")]
    public float tempoInvencibilidade = 1f;

    [Header("Pisar no inimigo")]
    public float forcaQuicarAoMatar = 5f;

    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRange = 1.2f;
    public int attackDamage = 1;

    [Header("Coleta de Erva Cura")]
    public int quantidadeErvaCura = 1;

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
    private bool isCrouching = false;

    private ErvaCuraPickup ervaCuraPerto;

    public bool podePassarAranhaBau = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (boxCollider != null)
        {
            if (colliderNormalSize == Vector2.zero)
                colliderNormalSize = boxCollider.size;

            if (colliderNormalOffset == Vector2.zero)
                colliderNormalOffset = boxCollider.offset;
        }

        if (clarkeAgachada != null)
            clarkeAgachada.SetActive(false);

        AtualizarVidas();

        if (rb != null)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
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

        // ========== ATAQUE COM MOUSE DIREITO ==========
        if (Input.GetMouseButtonDown(1))
        {
            Atacar();
        }

        // ========== PEGAR ERVA CURA COM E ==========
        if (Input.GetKeyDown(KeyCode.E) && ervaCuraPerto != null)
        {
            PegarErvaCura();
        }

        // ========== AGACHAMENTO ==========
        if (
            Input.GetKeyDown(KeyCode.C) ||
            Input.GetKeyDown(KeyCode.LeftControl) ||
            Input.GetKeyDown(KeyCode.RightControl)
        )
        {
            if (isCrouching)
                Levantar();
            else
                Agachar();
        }

        // ========== MOVIMENTO ==========
        float movimento = 0;

        if (Input.GetKey(KeyCode.D))
            movimento = 1;
        else if (Input.GetKey(KeyCode.A))
            movimento = -1;

        float velocidadeAtual = velocidade;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            velocidadeAtual = runSpeed;

        if (isCrouching)
            velocidadeAtual = crouchSpeed;

        rb.linearVelocity = new Vector2(movimento * velocidadeAtual, rb.linearVelocity.y);

        // ========== VIRAR PERSONAGEM ==========
        if (movimento != 0)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * (movimento > 0 ? 1 : -1);
            transform.localScale = escala;

            if (clarkeAgachada != null)
                AplicarFlip(clarkeAgachada);
        }

        AplicarLimites();

        // ========== PULO ==========
        if (!isCrouching && Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }

    void Atacar()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("AttackPoint não foi ligado no PlayerFase4.");
            return;
        }

        Collider2D[] objetosAtingidos = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange
        );

        bool acertouAlgo = false;

        foreach (Collider2D objeto in objetosAtingidos)
        {
            if (!objeto.CompareTag("inimigo"))
                continue;

            acertouAlgo = true;

            // ========== BRUXA ==========
            BruxaEnemy bruxa = objeto.GetComponent<BruxaEnemy>();

            if (bruxa == null)
                bruxa = objeto.GetComponentInParent<BruxaEnemy>();

            if (bruxa != null)
            {
                bruxa.TakeDamage(attackDamage);
                Debug.Log("Acertou a bruxa!");
                continue;
            }

            // ========== MORCEGO ==========
            BatEnemy morcego = objeto.GetComponent<BatEnemy>();

            if (morcego == null)
                morcego = objeto.GetComponentInParent<BatEnemy>();

            if (morcego != null)
            {
                morcego.TakeDamage(attackDamage);
                Debug.Log("Acertou o morcego!");
                continue;
            }

            // ========== ARANHA ==========
            Aranha aranha = objeto.GetComponent<Aranha>();

            if (aranha == null)
                aranha = objeto.GetComponentInParent<Aranha>();

            if (aranha != null)
            {
                aranha.ReceberDano();
                Debug.Log("Acertou a aranha!");
                continue;
            }

            // Se for inimigo, mas não tiver script conhecido
            Destroy(objeto.gameObject);
            Debug.Log("Inimigo destruído.");
        }

        if (!acertouAlgo)
        {
            Debug.Log("Ataque não acertou nenhum inimigo.");
        }
    }

    void PegarErvaCura()
    {
        if (ervaCuraPerto == null) return;

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ervaCura += quantidadeErvaCura;
            Debug.Log("Erva de cura adicionada ao inventário! Total: " + InventoryManager.Instance.ervaCura);
        }
        else
        {
            Debug.LogWarning("InventoryManager não encontrado.");
        }

        SistemaProgresso progresso = GetComponent<SistemaProgresso>();

        if (progresso != null && !string.IsNullOrEmpty(ervaCuraPerto.idItem))
        {
            progresso.ColetarItem(ervaCuraPerto.idItem);
            Debug.Log("Erva de cura registrada no progresso: " + ervaCuraPerto.idItem);
        }

        Destroy(ervaCuraPerto.gameObject);
        ervaCuraPerto = null;
    }

    void Agachar()
    {
        if (isCrouching) return;

        isCrouching = true;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (clarkeAgachada != null)
        {
            clarkeAgachada.SetActive(true);
            AplicarFlip(clarkeAgachada);
            clarkeAgachada.transform.localPosition = Vector3.zero;
        }

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

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (clarkeAgachada != null)
            clarkeAgachada.SetActive(false);

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

    bool EstaPisandoNoInimigo(Collision2D collision)
    {
        foreach (ContactPoint2D contato in collision.contacts)
        {
            if (contato.normal.y > 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    void MatarInimigo(GameObject inimigo)
    {
        Debug.Log("Pisou no inimigo! Inimigo derrotado.");

        Destroy(inimigo);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaQuicarAoMatar);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = true;
        }

        if (collision.gameObject.CompareTag("inimigo"))
        {
            if (EstaPisandoNoInimigo(collision))
            {
                MatarInimigo(collision.gameObject);
                return;
            }

            if (!invencivel)
            {
                Debug.Log("Encostou no inimigo de lado ou por baixo! Perdeu vida.");
                PerderVida();
                StartCoroutine(AtivarInvencibilidade());
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = true;
        }

        if (collision.gameObject.CompareTag("inimigo"))
        {
            if (EstaPisandoNoInimigo(collision))
            {
                MatarInimigo(collision.gameObject);
                return;
            }

            if (!invencivel)
            {
                Debug.Log("Continuou encostando no inimigo! Perdeu vida.");
                PerderVida();
                StartCoroutine(AtivarInvencibilidade());
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            estaNoChao = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ErvaCuraPickup erva = other.GetComponent<ErvaCuraPickup>();

        if (erva != null)
        {
            ervaCuraPerto = erva;
            Debug.Log("Erva de cura próxima. Aperte E para coletar.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ErvaCuraPickup erva = other.GetComponent<ErvaCuraPickup>();

        if (erva != null && erva == ervaCuraPerto)
        {
            ervaCuraPerto = null;
            Debug.Log("Saiu de perto da erva de cura.");
        }
    }

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

        if (vidasRestantes <= 0)
            Morrer();
    }

    public void Morrer()
    {
        Debug.Log("Player morreu! Carregando tela de derrota...");
        SceneManager.LoadScene(nomeCenaDerrota);
    }

    IEnumerator AtivarInvencibilidade()
    {
        invencivel = true;
        yield return new WaitForSeconds(tempoInvencibilidade);
        invencivel = false;
    }

    void AtualizarVidas()
    {
        if (vida5 != null) vida5.SetActive(vidasRestantes >= 5);
        if (vida4 != null) vida4.SetActive(vidasRestantes >= 4);
        if (vida3 != null) vida3.SetActive(vidasRestantes >= 3);
        if (vida2 != null) vida2.SetActive(vidasRestantes >= 2);
        if (vida1 != null) vida1.SetActive(vidasRestantes >= 1);
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}