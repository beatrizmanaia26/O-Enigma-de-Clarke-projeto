using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 5f;
    public float forcaPulo = 7f;
    
    [Header("Vidas")]
    public GameObject vida5;  // Arraste a imagem vida5 aqui
    public GameObject vida4;  // Arraste a imagem vida4 aqui
    public GameObject vida3;  // Arraste a imagem vida3 aqui
    public GameObject vida2;  // Arraste a imagem vida2 aqui
    public GameObject vida1;  // Arraste a imagem vida1 aqui
    
    private Rigidbody2D rb;
    private bool estaNoChao;
    private int vidasRestantes = 5;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Garante que todas as vidas estão visíveis no início
        AtualizarVidas();
    }
    
    void Update()
    {
        // Movimento com as teclas A e D
        float movimento = 0;
        
        if (Input.GetKey(KeyCode.D))
        {
            movimento = 1;  // Frente
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movimento = -1; // Trás
        }
        
        // Aplica a velocidade
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
        
        // Pulo com Espaço
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
    Debug.Log("COLIDIU COM: " + collision.gameObject.name);
    Debug.Log("TAG: " + collision.gameObject.tag);
    
    // Verifica se é inimigo (tag minúscula)
    if (collision.gameObject.CompareTag("inimigo"))
    {
        Debug.Log("É INIMIGO! PERDENDO VIDA...");
        PerderVida();
    }
        // Se encostar em algo com tag "Inimigo"
        if (collision.gameObject.CompareTag("inimigo"))
        {
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
        if (vidasRestantes <= 0) return; // Já morreu
        
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
        // Mostra/esconde as imagens conforme as vidas restantes
        vida5.SetActive(vidasRestantes >= 5);
        vida4.SetActive(vidasRestantes >= 4);
        vida3.SetActive(vidasRestantes >= 3);
        vida2.SetActive(vidasRestantes >= 2);
        vida1.SetActive(vidasRestantes >= 1);
    }
    
    void Morrer()
    {
        Debug.Log("Player morreu! Game Over!");
        // Opcional: desativa o player ou recarrega a fase
        gameObject.SetActive(false);
        // Ou recarregar a fase:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}