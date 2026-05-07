using UnityEngine;

public class PlayerFase2Real : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 7f;
    public KeyCode teclaCorrida = KeyCode.LeftShift;

    [Header("Vidas")]
    public GameObject vida5;
    public GameObject vida4;
    public GameObject vida3;
    public GameObject vida2;
    public GameObject vida1;

    private Rigidbody2D rb;

    private bool isGrounded;
    private bool estaNaRampa;

    private int vidasRestantes = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        AtualizarVidas();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        bool correndo = Input.GetKey(teclaCorrida);

        float velocidadeAtual = correndo ? runSpeed : speed;

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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
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

    void PerderVida()
    {
        if (vidasRestantes <= 0) return;

        vidasRestantes--;

        AtualizarVidas();

        Debug.Log("Perdeu vida! Vidas restantes: " + vidasRestantes);

        if (vidasRestantes <= 0)
        {
            Morrer();
        }
    }

    void AtualizarVidas()
    {
        vida5.SetActive(vidasRestantes >= 5);
        vida4.SetActive(vidasRestantes >= 4);
        vida3.SetActive(vidasRestantes >= 3);
        vida2.SetActive(vidasRestantes >= 2);
        vida1.SetActive(vidasRestantes >= 1);
    }

    void Morrer()
    {
        Debug.Log("Player morreu! Game Over!");
        gameObject.SetActive(false);
    }
}