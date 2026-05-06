using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [Header("Invencibilidade")]
    public float tempoInvencibilidade = 1f;  // Tempo que fica invencível após levar dano
    
    [Header("Telas")]
    public GameObject telaDerrota;  // Arraste a tela de derrota aqui
    
    private Rigidbody2D rb;
    private bool estaNoChao;
    private int vidasRestantes = 5;
    private bool invencivel = false;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        AtualizarVidas();
        
        // Garante que a tela de derrota começa desativada
        if (telaDerrota != null)
            telaDerrota.SetActive(false);
    }
    
    void Update()
    {
        // Movimento
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
        
        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Só perde vida se não estiver invencível
        if (collision.gameObject.CompareTag("inimigo") && !invencivel)
        {
            PerderVida();
            StartCoroutine(AtivarInvencibilidade());
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
    
    System.Collections.IEnumerator AtivarInvencibilidade()
    {
        invencivel = true;
        
        // Efeito de piscar (opcional)
        float tempoDecorrido = 0;
        while (tempoDecorrido < tempoInvencibilidade)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;
            
            yield return new WaitForSeconds(0.1f);
            tempoDecorrido += 0.1f;
        }
        
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        
        invencivel = false;
        Debug.Log("Invencibilidade acabou");
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
        
        // Desativa o player
        gameObject.SetActive(false);
        
        // Mostra tela de derrota
        if (telaDerrota != null)
        {
            telaDerrota.SetActive(true);
        }
        
        // Pausa o jogo (opcional)
        Time.timeScale = 0f;
    }
    
    // Função para reiniciar o jogo (botão da tela de derrota)
    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}