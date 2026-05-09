using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // Necessário para TextMeshProUGUI

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
    public float tempoInvencibilidade = 1f;
    
    [Header("Derrota")]
    public string nomeCenaDerrota = "TelaDerrota";
    
    [Header("Limites da Tela")]
    public Transform limiteEsquerda;
    public Transform limiteDireita;
    
    [Header("Cura com Erva")]
    public float tempoCuraAnimacao = 0.5f;
    
    [Header("UI de Mensagens")]
    public TextMeshProUGUI textoMensagens; // Arraste o TextMeshProUGUI aqui
    
    private Rigidbody2D rb;
    private bool estaNoChao;
    private int vidasRestantes = 5;
    private bool invencivel = false;
    private bool curando = false;
    private SpriteRenderer spriteRenderer;
    private Coroutine coroutineMensagem;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        AtualizarVidas();
        
        // Garante que o texto de mensagem comece desativado
        if (textoMensagens != null)
            textoMensagens.gameObject.SetActive(false);
    }
    
    void Update()
    {
        // ========== MOVIMENTO ==========
        float movimento = 0;
        if (Input.GetKey(KeyCode.D)) movimento = 1;
        else if (Input.GetKey(KeyCode.A)) movimento = -1;
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
        
        AplicarLimites();
        
        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
        
        // ========== USAR ERVA DE CURA (TECLA R) ==========
        if (Input.GetKeyDown(KeyCode.R) && !curando)
        {
            UsarErvaCura();
        }
    }
    
    void AplicarLimites()
    {
        if (limiteEsquerda == null || limiteDireita == null) return;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteEsquerda.position.x, limiteDireita.position.x);
        transform.position = pos;
    }
    
    // ========== COLISÕES COM INIMIGOS ==========
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("aranha"))
        {
            if (EstaEmCimaDoInimigo(collision))
            {
                MatarAranha(collision.gameObject);
                Debug.Log("Matou a aranha!");
            }
            else
            {
                if (!invencivel)
                {
                    PerderVida();
                    StartCoroutine(AtivarInvencibilidade());
                    Debug.Log("Encostou na aranha! Perdeu vida!");
                }
            }
        }
        else if (collision.gameObject.CompareTag("inimigo") && !invencivel)
        {
            PerderVida();
            StartCoroutine(AtivarInvencibilidade());
            Debug.Log("Encostou no inimigo! Perdeu vida!");
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
        if (scriptAranha != null)
            scriptAranha.ReceberDano();
        else
            Destroy(aranha);
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo * 0.5f);
    }
    
    // ========== SISTEMA DE VIDA E CURA ==========
    void PerderVida()
    {
        if (vidasRestantes <= 0) return;
        vidasRestantes--;
        AtualizarVidas();
        Debug.Log("Perdeu vida! Vidas restantes: " + vidasRestantes);
        if (vidasRestantes <= 0) Morrer();
    }
    
    void UsarErvaCura()
    {
        if (InventoryManager.Instance == null) return;
        
        int ervas = InventoryManager.Instance.ervaCura;
        int vidaAtual = vidasRestantes;
        
        // Verifica se tem erva
        if (ervas <= 0)
        {
            MostrarMensagem("❌ Você não tem erva de cura!");
            return;
        }
        
        // Verifica se precisa curar
        if (vidaAtual >= 5)
        {
            MostrarMensagem("❤️ Sua vida já está no máximo!");
            return;
        }
        
        // Aplica a cura: gasta 1 erva, aumenta 1 vida
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
        if (spriteRenderer != null)
        {
            Color original = spriteRenderer.color;
            spriteRenderer.color = Color.green;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = original;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.green;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = original;
        }
        curando = false;
    }
    
    IEnumerator AtivarInvencibilidade()
    {
        invencivel = true;
        float t = 0;
        while (t < tempoInvencibilidade)
        {
            if (spriteRenderer) spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }
        if (spriteRenderer) spriteRenderer.enabled = true;
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
        Debug.Log("Player morreu! Carregando tela de derrota...");
        SceneManager.LoadScene(nomeCenaDerrota);
    }
    
    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // ========== MÉTODOS PARA O SISTEMA DE SAVE ==========
    
    // Propriedade pública para acessar as vidas restantes (usada pelo sistema de save)
    public int VidasRestantes 
    { 
        get { return vidasRestantes; }
        set 
        { 
            vidasRestantes = Mathf.Clamp(value, 0, 5); 
            AtualizarVidas();
        }
    }
    
    // Método para obter a posição do jogador (usada pelo sistema de save)
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    // Método para definir a posição do jogador (usada pelo sistema de restore)
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}