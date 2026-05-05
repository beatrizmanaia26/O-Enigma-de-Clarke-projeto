using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidade = 5f;
    public float forcaPulo = 7f;
    
    private Rigidbody2D rb;
    private bool estaNoChao;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Movimento com as teclas A e D
        float movimento = 0;
        
        if (Input.GetKey(KeyCode.D))
        {
            movimento = 1;  // Frente
            Debug.Log("D pressionado - andando pra frente");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movimento = -1; // Trás
            Debug.Log("A pressionado - andando pra trás");
        }
        
        // Aplica a velocidade
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
        
        // Pulo com Espaço
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            Debug.Log("Pulou!");
        }
    }
    
    // Detecta quando está no chão
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
}