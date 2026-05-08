using UnityEngine;

public class AranhaMove : MonoBehaviour
{
    public string mensagemSemItem = "⚠️ Você não tem todos os itens para passar! ⚠️";
    
    private float tempoMensagem = 0;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player encostou na aranha!");
            
            // Verificação DIRETA do InventoryManager
            if (InventoryManager.Instance == null)
            {
                Debug.LogError("InventoryManager não encontrado!");
                return;
            }
            
            Debug.Log($"Quantidade de coroas: {InventoryManager.Instance.crowns}");
            
            if (InventoryManager.Instance.crowns > 0)
            {
                // TEM COROA - ARANHA SOME
                Debug.Log("★★★ TEM COROA! ARANHA VAI SUMIR! ★★★");
                Destroy(gameObject);
            }
            else
            {
                // NÃO TEM COROA
                if (Time.time > tempoMensagem)
                {
                    Debug.LogWarning(mensagemSemItem);
                    tempoMensagem = Time.time + 2f;
                }
                
                // Empurra o player de volta
                Vector2 direcao = (collision.transform.position - transform.position).normalized;
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(direcao.x * -3f, playerRb.linearVelocity.y);
                }
            }
        }
    }
}