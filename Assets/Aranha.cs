using UnityEngine;
using System.Collections;

public class Aranha : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidadeBase = 2f;
    public Transform limiteEsquerda;
    public Transform limiteDireita;
    
    [Header("Bloqueio")]
    public bool isBlocking = true;  // Se é um bloqueio
    public string itemNecessario = "crown";  // Item necessário (crown)
    public int quantidadeNecessaria = 1;  // Quantidade necessária
    
    private float velocidadeAtual;
    private bool movendoParaDireita = true;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private int vida = 1;
    private bool bloqueioAtivo = true;
    private Collider2D colisor;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        colisor = GetComponent<Collider2D>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        velocidadeAtual = velocidadeBase;
        
        if (AranhaManager.Instance != null)
            AranhaManager.Instance.RegistrarAranha(this);
        
        // Se for bloqueio, começa ativo
        if (isBlocking)
        {
            bloqueioAtivo = true;
            // Muda a cor para indicar que é bloqueio
            if (sr != null)
                sr.color = new Color(0.8f, 0.3f, 0.3f); // Vermelho escuro
        }
    }
    
    void Update()
    {
        if (!isBlocking || !bloqueioAtivo)
        {
            Mover(); // Só se move se não for bloqueio ou se já foi desbloqueado
        }
    }
    
    void Mover()
    {
        if (limiteEsquerda == null || limiteDireita == null) return;
        float direcao = movendoParaDireita ? 1f : -1f;
        rb.linearVelocity = new Vector2(direcao * velocidadeAtual, rb.linearVelocity.y);
        
        if (movendoParaDireita && transform.position.x >= limiteDireita.position.x)
        {
            movendoParaDireita = false;
            Virar();
        }
        else if (!movendoParaDireita && transform.position.x <= limiteEsquerda.position.x)
        {
            movendoParaDireita = true;
            Virar();
        }
    }
    
    void Virar()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
    
    // Verifica se o player pode passar
    public bool PodePassar()
    {
        if (!isBlocking || !bloqueioAtivo)
            return true; // Não é bloqueio ou já foi desbloqueado
        
        // Verifica se tem o item no inventário
        if (InventoryManager.Instance != null)
        {
            bool temItem = false;
            
            switch (itemNecessario.ToLower())
            {
                case "crown":
                case "coroa":
                    temItem = InventoryManager.Instance.crowns >= quantidadeNecessaria;
                    break;
                case "coin":
                case "moeda":
                    temItem = InventoryManager.Instance.coins >= quantidadeNecessaria;
                    break;
                case "star":
                case "estrela":
                    temItem = InventoryManager.Instance.stars >= quantidadeNecessaria;
                    break;
                case "bluestone":
                case "pedra":
                    temItem = InventoryManager.Instance.blueStones >= quantidadeNecessaria;
                    break;
                case "flor":
                    temItem = InventoryManager.Instance.flor >= quantidadeNecessaria;
                    break;
            }
            
            if (temItem)
            {
                Desbloquear();
                return true;
            }
        }
        
        return false;
    }
    
    void Desbloquear()
{
    bloqueioAtivo = false;
    isBlocking = false;
    
    if (sr != null)
        sr.color = new Color(0.3f, 0.8f, 0.3f);
        
    // Consome a coroa
    if (InventoryManager.Instance != null)
    {
        InventoryManager.Instance.SpendCrown(quantidadeNecessaria);
        Debug.Log($"Consumiu {quantidadeNecessaria} coroa(s)!");
    }
}
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isBlocking && bloqueioAtivo)
            {
                if (!PodePassar())
                {
                    // Bloqueia o movimento do player
                    BloquearPlayer(collision.gameObject);
                }
            }
        }
    }
    
    void BloquearPlayer(GameObject player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            // Empurra o player de volta
            Vector2 direcaoBloqueio = (player.transform.position - transform.position).normalized;
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
            
            // Mostra mensagem
            Debug.Log($"Precisa de {quantidadeNecessaria} {itemNecessario} para passar!");
        }
    }
    
    public void ReceberDano()
    {
        if (isBlocking && bloqueioAtivo)
        {
            Debug.Log("A aranha bloqueio não pode ser morta! Use a coroa para passar.");
            return;
        }
        
        vida--;
        if (vida <= 0)
            Morrer();
        else
            StartCoroutine(Piscar());
    }
    
    void Morrer()
    {
        if (AranhaManager.Instance != null)
        {
            AranhaManager.Instance.RemoverAranha(this);
            AranhaManager.Instance.AoMatarAranha();
        }
        Destroy(gameObject);
    }
    
    public void AumentarVelocidade()
    {
        if (!isBlocking)
        {
            velocidadeAtual = velocidadeBase * 1.5f;
            if (sr != null)
                sr.color = new Color(1f, 0.5f, 0.5f);
        }
    }
    
    public void TornarResistente()
    {
        if (!isBlocking)
        {
            vida = 2;
            if (sr != null)
                sr.color = new Color(0.5f, 0.5f, 1f);
        }
    }
    
    IEnumerator Piscar()
    {
        if (sr == null) yield break;
        Color original = sr.color;
        for (int i = 0; i < 3; i++)
        {
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            sr.color = original;
            yield return new WaitForSeconds(0.1f);
        }
    }
}