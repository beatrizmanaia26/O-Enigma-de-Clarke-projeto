// using UnityEngine;

// public class PlayerFase2 : MonoBehaviour
// {
//     [Header("Movimento")]
//     public float speed = 5f;
//     public float runSpeed = 8f;
//     public float crouchSpeed = 2f;
//     public float jumpForce = 7f;

//     [Header("Visual agachado")]
//     public GameObject clarkeAgachada;

//     [Header("Collider normal")]
//     public Vector2 colliderNormalSize;
//     public Vector2 colliderNormalOffset;

//     [Header("Collider agachado")]
//     public Vector2 colliderAgachadoSize;
//     public Vector2 colliderAgachadoOffset;

//     [Header("Vidas")]
//     public GameObject vida5;
//     public GameObject vida4;
//     public GameObject vida3;
//     public GameObject vida2;
//     public GameObject vida1;

//     private Rigidbody2D rb;
//     private BoxCollider2D boxCollider;
//     private PlayerVisualSwap visualSwap;

//     private bool isGrounded;
//     private bool isCrouching;
//     private bool viradoParaDireita = true;

//     private int vidasRestantes = 5;

//     public bool IsCrouching
//     {
//         get { return isCrouching; }
//     }

//     public bool ViradoParaDireita
//     {
//         get { return viradoParaDireita; }
//     }

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         boxCollider = GetComponent<BoxCollider2D>();
//         visualSwap = GetComponent<PlayerVisualSwap>();

//         if (boxCollider != null)
//         {
//             if (colliderNormalSize == Vector2.zero)
//                 colliderNormalSize = boxCollider.size;

//             if (colliderNormalOffset == Vector2.zero)
//                 colliderNormalOffset = boxCollider.offset;
//         }

//         if (clarkeAgachada != null)
//             clarkeAgachada.SetActive(false);

//         if (visualSwap != null)
//             visualSwap.AtualizarVisual(viradoParaDireita);

//         AtualizarVidas();
//     }

//     void Update()
//     {
//         Crouch();
//         Move();
//         Jump();

//         if (!isCrouching && visualSwap != null)
//         {
//             visualSwap.AtualizarVisual(viradoParaDireita);
//         }
//     }

//     void Move()
//     {
//         float moveInput = Input.GetAxis("Horizontal");

//         VirarPersonagem(moveInput);

//         float currentSpeed = speed;

//         if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
//         {
//             currentSpeed = runSpeed;
//         }

//         if (isCrouching)
//         {
//             currentSpeed = crouchSpeed;
//         }

//         rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
//     }

//     void Jump()
//     {
//         if (isCrouching) return;

//         if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//         {
//             rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//         }
//     }

//     void Crouch()
//     {
//         if (
//             Input.GetKeyDown(KeyCode.C) ||
//             Input.GetKeyDown(KeyCode.LeftControl) ||
//             Input.GetKeyDown(KeyCode.RightControl)
//         )
//         {
//             if (isCrouching)
//                 Levantar();
//             else
//                 Agachar();
//         }
//     }

//     void Agachar()
//     {
//         isCrouching = true;

//         if (visualSwap != null)
//             visualSwap.EsconderVisuaisNormais();

//         if (clarkeAgachada != null)
//         {
//             clarkeAgachada.SetActive(true);
//             AplicarFlip(clarkeAgachada);
//         }

//         if (boxCollider != null)
//         {
//             boxCollider.size = colliderAgachadoSize;
//             boxCollider.offset = colliderAgachadoOffset;
//         }
//     }

//     void Levantar()
//     {
//         isCrouching = false;

//         if (clarkeAgachada != null)
//             clarkeAgachada.SetActive(false);

//         if (boxCollider != null)
//         {
//             boxCollider.size = colliderNormalSize;
//             boxCollider.offset = colliderNormalOffset;
//         }

//         if (visualSwap != null)
//             visualSwap.AtualizarVisual(viradoParaDireita);
//     }

//     void VirarPersonagem(float moveInput)
//     {
//         if (moveInput > 0)
//             viradoParaDireita = true;
//         else if (moveInput < 0)
//             viradoParaDireita = false;

//         if (visualSwap != null && !isCrouching)
//             visualSwap.AtualizarVisual(viradoParaDireita);

//         AplicarFlip(clarkeAgachada);
//     }

//     void AplicarFlip(GameObject visual)
//     {
//         if (visual == null) return;

//         SpriteRenderer spriteRenderer = visual.GetComponent<SpriteRenderer>();

//         if (spriteRenderer != null)
//             spriteRenderer.flipX = !viradoParaDireita;
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Ground"))
//             isGrounded = true;
//     }

//     private void OnCollisionExit2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Ground"))
//             isGrounded = false;
//     }

//     public void PerderVida()
//     {
//         if (vidasRestantes <= 0) return;

//         vidasRestantes--;
//         AtualizarVidas();

//         Debug.Log("Perdeu vida! Vidas restantes: " + vidasRestantes);

//         if (vidasRestantes <= 0)
//             Morrer();
//     }

//     void AtualizarVidas()
//     {
//         if (vida5 != null) vida5.SetActive(vidasRestantes >= 5);
//         if (vida4 != null) vida4.SetActive(vidasRestantes >= 4);
//         if (vida3 != null) vida3.SetActive(vidasRestantes >= 3);
//         if (vida2 != null) vida2.SetActive(vidasRestantes >= 2);
//         if (vida1 != null) vida1.SetActive(vidasRestantes >= 1);
//     }

//     void Morrer()
//     {
//         Debug.Log("Player morreu! Game Over!");
//         gameObject.SetActive(false);
//     }
// }


using UnityEngine;

public class PlayerFase2 : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float jumpForce = 7f;

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

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private PlayerVisualSwap visualSwap;

    private bool isGrounded;
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
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        VirarPersonagem(moveInput);

        float currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = runSpeed;
        }

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
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

        if (visualSwap != null)
            visualSwap.EsconderVisuaisNormais();

        if (clarkeAgachada != null)
        {
            clarkeAgachada.SetActive(true);
            AplicarFlip(clarkeAgachada);
        }

        if (boxCollider != null)
        {
            boxCollider.size = colliderAgachadoSize;
            boxCollider.offset = colliderAgachadoOffset;
        }
    }

    void Levantar()
    {
        isCrouching = false;

        if (clarkeAgachada != null)
            clarkeAgachada.SetActive(false);

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
            viradoParaDireita = true;
        else if (moveInput < 0)
            viradoParaDireita = false;

        if (visualSwap != null && !isCrouching)
            visualSwap.AtualizarVisual(viradoParaDireita);

        AplicarFlip(clarkeAgachada);
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
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
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
            Morrer();
    }

    void AtualizarVidas()
    {
        if (vida5 != null) vida5.SetActive(vidasRestantes >= 5);
        if (vida4 != null) vida4.SetActive(vidasRestantes >= 4);
        if (vida3 != null) vida3.SetActive(vidasRestantes >= 3);
        if (vida2 != null) vida2.SetActive(vidasRestantes >= 2);
        if (vida1 != null) vida1.SetActive(vidasRestantes >= 1);
    }

    void Morrer()
    {
        Debug.Log("Player morreu! Game Over!");
        gameObject.SetActive(false);
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