using UnityEngine;

public class PlayerFase2 : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;
    public float jumpForce = 7f;

    [Header("Vidas")]
    public GameObject vida5;
    public GameObject vida4;
    public GameObject vida3;
    public GameObject vida2;
    public GameObject vida1;

    private Rigidbody2D rb;
    private bool isGrounded;
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
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
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
        // CHÃO
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void PerderVida()
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
}