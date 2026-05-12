using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFase2Real : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;
    public float runSpeed = 9f;
    public float crouchSpeed = 2f;
    public float jumpForce = 7f;
    public KeyCode teclaCorrida = KeyCode.LeftShift;

    [Header("Visual normal")]
    public GameObject visualNormal;

    [Header("Visual agachado")]
    public GameObject clarkeAgachada;

    [Header("Collider normal")]
    public Vector2 colliderNormalSize;
    public Vector2 colliderNormalOffset;

    [Header("Collider agachado")]
    public Vector2 colliderAgachadoSize;
    public Vector2 colliderAgachadoOffset;

    [Header("Vidas")]
    public GameObject vida5;
    public GameObject vida4;
    public GameObject vida3;
    public GameObject vida2;
    public GameObject vida1;

    [Header("Derrota")]
    public string nomeCenaDerrota = "TelaDerrota";

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private PlayerVisualSwap visualSwap;

    private bool isGrounded;
    private bool estaNaRampa;
    private bool isCrouching;
    private bool viradoParaDireita = true;

    private int vidasRestantes = 5;

    public bool IsCrouching
    {
        get { return isCrouching; }
    }

    public bool ViradoParaDireita
    {
        get { return viradoParaDireita; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        visualSwap = GetComponent<PlayerVisualSwap>();

        if (boxCollider != null)
        {
            if (colliderNormalSize == Vector2.zero)
                colliderNormalSize = boxCollider.size;

            if (colliderNormalOffset == Vector2.zero)
                colliderNormalOffset = boxCollider.offset;
        }

        if (clarkeAgachada != null)
            clarkeAgachada.SetActive(false);

        if (visualNormal != null)
            visualNormal.SetActive(true);

        if (visualSwap != null)
            visualSwap.AtualizarVisual(viradoParaDireita);

        if (GameStateManager.Instance != null)
        {
            vidasRestantes = GameStateManager.Instance.vidasGlobais;
        }

        AtualizarVidas();
    }

    void Update()
    {
        Crouch();
        Move();
        Jump();

        if (!isCrouching && visualSwap != null)
        {
            visualSwap.AtualizarVisual(viradoParaDireita);
        }

        // CURAR COM R
        if (Input.GetKeyDown(KeyCode.R))
        {
            UsarErvaCura();
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        VirarPersonagem(moveInput);

        bool correndo = Input.GetKey(teclaCorrida);

        float velocidadeAtual = correndo ? runSpeed : speed;

        if (isCrouching)
        {
            velocidadeAtual = crouchSpeed;
        }

        // BLOQUEIA SUBIDA DA RAMPA SEM CORRER
        if (estaNaRampa && !correndo && moveInput > 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * velocidadeAtual, rb.linearVelocity.y);
    }

    void Jump()
    {
        // Não deixa pular agachada
        if (isCrouching) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void Crouch()
    {
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
    }

    void Agachar()
    {
        isCrouching = true;

        // Esconde o sprite normal
        if (visualNormal != null)
            visualNormal.SetActive(false);

        // Caso você ainda use PlayerVisualSwap, também tenta esconder os visuais normais
        if (visualSwap != null)
            visualSwap.EsconderVisuaisNormais();

        // Mostra o sprite agachado
        if (clarkeAgachada != null)
        {
            clarkeAgachada.SetActive(true);
            AplicarFlip(clarkeAgachada);
        }

        // Ajusta collider para agachado
        if (boxCollider != null)
        {
            boxCollider.size = colliderAgachadoSize;
            boxCollider.offset = colliderAgachadoOffset;
        }
    }

    void Levantar()
    {
        isCrouching = false;

        // Esconde o sprite agachado
        if (clarkeAgachada != null)
            clarkeAgachada.SetActive(false);

        // Mostra o sprite normal
        if (visualNormal != null)
            visualNormal.SetActive(true);

        // Volta collider normal
        if (boxCollider != null)
        {
            boxCollider.size = colliderNormalSize;
            boxCollider.offset = colliderNormalOffset;
        }

        if (visualSwap != null)
            visualSwap.AtualizarVisual(viradoParaDireita);
    }

    void VirarPersonagem(float moveInput)
    {
        if (moveInput > 0)
        {
            viradoParaDireita = true;

            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            viradoParaDireita = false;

            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void AplicarFlip(GameObject visual)
    {
        if (visual == null) return;

        SpriteRenderer spriteRenderer = visual.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.flipX = !viradoParaDireita;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // CHÃO NORMAL
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // RAMPA
        if (collision.gameObject.CompareTag("Ramp"))
        {
            isGrounded = true;
            estaNaRampa = true;
        }

        // INIMIGO
        if (collision.gameObject.CompareTag("inimigo"))
        {
            PerderVida();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Ramp"))
        {
            isGrounded = true;
            estaNaRampa = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Ramp"))
        {
            estaNaRampa = false;
        }
    }

    public void PerderVida()
    {
        if (vidasRestantes <= 0) return;

        vidasRestantes--;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.vidasGlobais = vidasRestantes;
        }

        AtualizarVidas();

        Debug.Log("Perdeu vida! Vidas restantes: " + vidasRestantes);

        if (vidasRestantes <= 0)
        {
            Morrer();
        }
    }

    void AtualizarVidas()
    {
        if (vida5 != null) vida5.SetActive(vidasRestantes >= 5);
        if (vida4 != null) vida4.SetActive(vidasRestantes >= 4);
        if (vida3 != null) vida3.SetActive(vidasRestantes >= 3);
        if (vida2 != null) vida2.SetActive(vidasRestantes >= 2);
        if (vida1 != null) vida1.SetActive(vidasRestantes >= 1);
    }

    void UsarErvaCura()
    {
        if (InventoryManager.Instance == null)
            return;

        if (InventoryManager.Instance.ervaCura <= 0)
        {
            Debug.Log("Sem erva de cura!");
            return;
        }

        if (vidasRestantes >= 5)
        {
            Debug.Log("Vida já está cheia!");
            return;
        }

        InventoryManager.Instance.SpendErvaCura(1);

        vidasRestantes++;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.vidasGlobais = vidasRestantes;
        }

        AtualizarVidas();

        Debug.Log("Curou 1 vida!");
    }

    void Morrer()
    {
        Debug.Log("Player morreu! Game Over!");
        gameObject.SetActive(false);
        SceneManager.LoadScene(nomeCenaDerrota);
    }

    // ========== MÉTODOS PARA SISTEMA DE SAVE ==========
    public int VidasRestantes
    {
        get { return vidasRestantes; }
        set
        {
            vidasRestantes = Mathf.Clamp(value, 0, 5);

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.vidasGlobais = vidasRestantes;
            }

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