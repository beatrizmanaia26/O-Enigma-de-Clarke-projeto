using UnityEngine;

public class AranhaMove : MonoBehaviour
{
    [Header("Configuração")]
    public string itemNecessario = "crown";  // Item necessário para passar
    public int quantidadeNecessaria = 1;
    public string mensagemSemItem = "⚠️ VOCÊ PRECISA DE UMA COROA PARA PASSAR! ⚠️";
    public string mensagemComItem = "✅ COROA DETECTADA! PODE PASSAR! ✅";
    
    [Header("Cores (opcional)")]
    public Color corBloqueado = new Color(1f, 0.3f, 0.3f);  // Vermelho
    public Color corLiberado = new Color(0.3f, 1f, 0.3f);    // Verde
    
    private SpriteRenderer sr;
    private bool playerPodePassar = false;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        // Começa bloqueado
        if (sr != null)
            sr.color = corBloqueado;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            VerificarEAcao(collision.gameObject);
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            VerificarEAcao(collision.gameObject);
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Saiu do baú, desbloqueia
            PlayerFase4 player = collision.gameObject.GetComponent<PlayerFase4>();
            if (player != null)
            {
                player.Desbloquear();
            }
            playerPodePassar = false;
        }
    }
    
    void VerificarEAcao(GameObject player)
    {
        // Verifica se tem coroa no inventário
        if (TemCoroa())
        {
            // TEM COROA - pode passar
            if (!playerPodePassar)
            {
                Debug.Log(mensagemComItem);
                playerPodePassar = true;
                
                // Muda a cor para verde (liberado)
                if (sr != null)
                    sr.color = corLiberado;
            }
            
            // Desbloqueia o player
            PlayerFase4 playerScript = player.GetComponent<PlayerFase4>();
            if (playerScript != null)
            {
                playerScript.Desbloquear();
                playerScript.podePassarAranhaBau = true; // Variável extra se precisar
            }
        }
        else
        {
            // NÃO TEM COROA - bloqueia
            Debug.LogWarning(mensagemSemItem);
            
            // Muda a cor para vermelho (bloqueado)
            if (sr != null)
                sr.color = corBloqueado;
            
            // Bloqueia o player
            PlayerFase4 playerScript = player.GetComponent<PlayerFase4>();
            if (playerScript != null)
            {
                playerScript.Bloquear();
            }
            
            // Empurra o player de volta
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 direcao = (player.transform.position - transform.position).normalized;
                playerRb.linearVelocity = new Vector2(direcao.x * -3f, playerRb.linearVelocity.y);
            }
        }
    }
    
    bool TemCoroa()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager não encontrado!");
            return false;
        }
        
        switch (itemNecessario.ToLower())
        {
            case "crown":
            case "coroa":
                return InventoryManager.Instance.crowns >= quantidadeNecessaria;
            case "coin":
            case "moeda":
                return InventoryManager.Instance.coins >= quantidadeNecessaria;
            case "star":
            case "estrela":
                return InventoryManager.Instance.stars >= quantidadeNecessaria;
            default:
                return InventoryManager.Instance.crowns >= quantidadeNecessaria;
        }
    }
    
    // Método para consumir a coroa (opcional - chamar quando passar)
    public void ConsumirCoroa()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.crowns >= quantidadeNecessaria)
        {
            InventoryManager.Instance.SpendCrown(quantidadeNecessaria);
            Debug.Log($"Consumiu {quantidadeNecessaria} coroa(s)!");
        }
    }
}