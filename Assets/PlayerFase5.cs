using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public float runSpeed = 8f;
    public float forcaPulo = 7f;

    [Header("Agachamento (OBRIGATÓRIO)")]
    [Tooltip("Arraste o GameObject com a sprite do Clarke agachado")]
    public GameObject clarkeAgachada;

    [Tooltip("Velocidade enquanto agachado (ex: 2)")]
    public float crouchSpeed = 2f;

    [Header("Collider Normal (valores atuais preenchidos automaticamente)")]
    public Vector2 colliderNormalSize;
    public Vector2 colliderNormalOffset;

    [Header("Collider Agachado (OBRIGATÓRIO - preencha manualmente!)")]
    [Tooltip("Tamanho do BoxCollider2D quando agachado (ex: (0.8, 0.8))")]
    public Vector2 colliderAgachadoSize;

    [Tooltip("Offset do BoxCollider2D quando agachado (ex: (0, 0.2))")]
    public Vector2 colliderAgachadoOffset;

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

    [Header("Cura com Erva")]
    public float tempoCuraAnimacao = 0.5f;

    [Header("UI de Mensagens")]
    public TextMeshProUGUI textoMensagens;

    // Componentes privados
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRendererNormal;
    private bool estaNoChao;
    private int vidasRestantes = 5;
    private bool invencivel = false;
    private bool curando = false;
    private bool isCrouching = false;
    private Coroutine coroutineMensagem;

    // Flag para verificar se o agachamento foi configurado corretamente
    private bool crouchConfigurado = true;

    void OnValidate()
    {
        // Verifica no Editor se os campos obrigatórios estão preenchidos
        if (clarkeAgachada == null)
            Debug.LogWarning("Player: O campo 'clarkeAgachada' (GameObject agachado) não foi atribuído!");

        if (colliderAgachadoSize == Vector2.zero)
            Debug.LogWarning("Player: 'colliderAgachadoSize' está com valor zero! Defina um tamanho apropriado para o collider agachado.");

        if (colliderAgachadoOffset == Vector2.zero)
            Debug.LogWarning("Player: 'colliderAgachadoOffset' está com valor zero! Defina um offset apropriado para o collider agachado.");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRendererNormal = GetComponent<SpriteRenderer>();

        // Salvar tamanhos normais do collider se não fornecidos
        if (boxCollider != null)
        {
            if (colliderNormalSize == Vector2.zero)
                colliderNormalSize = boxCollider.size;
            if (colliderNormalOffset == Vector2.zero)
                colliderNormalOffset = boxCollider.offset;
        }

        // Validação dos campos obrigatórios
        if (clarkeAgachada == null)
        {
            Debug.LogError("Player: O GameObject 'clarkeAgachada' não foi atribuído! O agachamento será desativado.");
            crouchConfigurado = false;
        }

        if (colliderAgachadoSize == Vector2.zero)
        {
            Debug.LogError("Player: 'colliderAgachadoSize' não foi definido! O agachamento será desativado.");
            crouchConfigurado = false;
        }

        if (colliderAgachadoOffset == Vector2.zero)
        {
            Debug.LogError("Player: 'colliderAgachadoOffset' não foi definido! O agachamento será desativado.");
            crouchConfigurado = false;
        }

        // Se alguma configuração essencial faltar, desabilita o agachamento
        if (!crouchConfigurado)
        {
            Debug.LogWarning("Player: Agachamento desabilitado devido a configurações incompletas.");
            clarkeAgachada?.SetActive(false);
            if (spriteRendererNormal != null)
                spriteRendererNormal.enabled = true;
        }
        else
        {
            // Inicia com o visual normal ativado e o agachado desativado
            if (clarkeAgachada != null)
                clarkeAgachada.SetActive(false);
            if (spriteRendererNormal != null)
                spriteRendererNormal.enabled = true;
        }

        AtualizarVidas();
        if (textoMensagens != null)
            textoMensagens.gameObject.SetActive(false);
    }

    void Update()
    {
        // Agachamento apenas se configurado
        if (crouchConfigurado)
        {
            if (Input.GetKeyDown(KeyCode.C) ||
                Input.GetKeyDown(KeyCode.LeftControl) ||
                Input.GetKeyDown(KeyCode.RightControl))
            {
                if (isCrouching)
                    Levantar();
                else
                    Agachar();
            }
        }

        // Movimento
        float movimento = 0;
        if (Input.GetKey(KeyCode.D)) movimento = 1;
        else if (Input.GetKey(KeyCode.A)) movimento = -1;

        float velocidadeAtual = velocidade;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            velocidadeAtual = runSpeed;
        if (isCrouching)
            velocidadeAtual = crouchSpeed;

        rb.linearVelocity = new Vector2(movimento * velocidadeAtual, rb.linearVelocity.y);

        // Virar
        if (movimento != 0)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * (movimento > 0 ? 1 : -1);
            transform.localScale = escala;

            if (crouchConfigurado && clarkeAgachada != null)
                AplicarFlip(clarkeAgachada);
        }

        AplicarLimites();

        // Pulo bloqueado quando agachado
        if (!isCrouching && Input.GetKeyDown(KeyCode.Space) && estaNoChao)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);

        // Usar erva
        if (Input.GetKeyDown(KeyCode.R) && !curando)
            UsarErvaCura();
    }

    void Agachar()
    {
        if (!crouchConfigurado || isCrouching) return;
        isCrouching = true;

        // Desliga o visual normal, liga o agachado
        if (spriteRendererNormal != null)
            spriteRendererNormal.enabled = false;
        if (clarkeAgachada != null)
        {
            clarkeAgachada.SetActive(true);
            AplicarFlip(clarkeAgachada);
            clarkeAgachada.transform.localPosition = Vector3.zero; // alinha com o personagem
        }

        // Ajusta o collider para o modo agachado (valores obrigatórios)
        if (boxCollider != null)
        {
            boxCollider.size = colliderAgachadoSize;
            boxCollider.offset = colliderAgachadoOffset;
        }
    }

    void Levantar()
    {
        if (!crouchConfigurado || !isCrouching) return;
        isCrouching = false;

        // Liga o visual normal, desliga o agachado
        if (spriteRendererNormal != null)
            spriteRendererNormal.enabled = true;
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

    // ... Os demais métodos (AplicarLimites, colisões, vidas, etc) permanecem iguais ...
    // Reproduzo abaixo apenas para completar, mas você pode mantê-los do seu script atual.

    void AplicarLimites()
    {
        if (limiteEsquerda == null || limiteDireita == null) return;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteEsquerda.position.x, limiteDireita.position.x);
        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao")) estaNoChao = true;

        if (collision.gameObject.CompareTag("aranha"))
        {
            if (EstaEmCimaDoInimigo(collision))
                MatarAranha(collision.gameObject);
            else if (!invencivel)
            {
                PerderVida();
                StartCoroutine(AtivarInvencibilidade());
            }
        }
        else if (collision.gameObject.CompareTag("inimigo") && !invencivel)
        {
            PerderVida();
            StartCoroutine(AtivarInvencibilidade());
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
        if (scriptAranha != null) scriptAranha.ReceberDano();
        else Destroy(aranha);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo * 0.5f);
    }

    void PerderVida()
    {
        if (vidasRestantes <= 0) return;
        vidasRestantes--;
        AtualizarVidas();
        if (vidasRestantes <= 0) Morrer();
    }

    void UsarErvaCura()
    {
        if (InventoryManager.Instance == null) return;
        int ervas = InventoryManager.Instance.ervaCura;
        if (ervas <= 0) { MostrarMensagem("❌ Você não tem erva de cura!"); return; }
        if (vidasRestantes >= 5) { MostrarMensagem("❤️ Sua vida já está no máximo!"); return; }

        InventoryManager.Instance.SpendErvaCura(1);
        vidasRestantes = Mathf.Min(vidasRestantes + 1, 5);
        AtualizarVidas();
        StartCoroutine(EfeitoCura());
        MostrarMensagem("🌿 +1 Vida recuperada!");
    }

    void MostrarMensagem(string msg)
    {
        if (textoMensagens == null) return;
        if (coroutineMensagem != null) StopCoroutine(coroutineMensagem);
        textoMensagens.text = msg;
        textoMensagens.gameObject.SetActive(true);
        coroutineMensagem = StartCoroutine(DesativarMensagem(2f));
    }

    IEnumerator DesativarMensagem(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        if (textoMensagens != null) textoMensagens.gameObject.SetActive(false);
        coroutineMensagem = null;
    }

    IEnumerator EfeitoCura()
    {
        curando = true;
        if (spriteRendererNormal != null)
        {
            Color original = spriteRendererNormal.color;
            spriteRendererNormal.color = Color.green;
            yield return new WaitForSeconds(0.2f);
            spriteRendererNormal.color = original;
            yield return new WaitForSeconds(0.2f);
            spriteRendererNormal.color = Color.green;
            yield return new WaitForSeconds(0.2f);
            spriteRendererNormal.color = original;
        }
        curando = false;
    }

    IEnumerator AtivarInvencibilidade()
    {
        invencivel = true;
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

    void Morrer()
    {
        SceneManager.LoadScene(nomeCenaDerrota);
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int VidasRestantes
    {
        get => vidasRestantes;
        set { vidasRestantes = Mathf.Clamp(value, 0, 5); AtualizarVidas(); }
    }

    public Vector3 GetPosition() => transform.position;
    public void SetPosition(Vector3 position) => transform.position = position;
}