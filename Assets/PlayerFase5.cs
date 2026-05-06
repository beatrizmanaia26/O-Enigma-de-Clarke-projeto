using UnityEngine;

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
    
    private Rigidbody2D rb;
    private bool estaNoChao;
    private int vidasRestantes = 5;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        AtualizarVidas();
    }
    
    void Update()
    {
        float movimento = 0;
        
        if (Input.GetKey(KeyCode.D))
        {
            movimento = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movimento = -1;
        }
        
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLIDIU COM: " + collision.gameObject.name);
        Debug.Log("TAG: " + collision.gameObject.tag);
        
        // Verifica se é inimigo - APENAS UMA VEZ!
        if (collision.gameObject.CompareTag("inimigo"))
        {
            Debug.Log("É INIMIGO! PERDENDO VIDA...");
            PerderVida();
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