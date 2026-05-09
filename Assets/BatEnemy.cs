using UnityEngine;

public class BatEnemy : MonoBehaviour
{
    public float speed = 3f;
    public int health = 2;
    
    [Header("Save System")]
    public string idInimigo = ""; // ID único para save

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Criar ID único se não tiver
        if (string.IsNullOrEmpty(idInimigo))
            idInimigo = $"bat_{gameObject.name}_{transform.position.x}_{transform.position.y}";
    }

    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // REGISTRAR NO SISTEMA DE PROGRESSO
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            SistemaProgresso progresso = playerObj.GetComponent<SistemaProgresso>();
            if (progresso != null)
                progresso.DerrotarInimigo(idInimigo);
        }
        
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verifica qual script o player tem (Player ou PlayerFase2Real)
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                // Se for o Player normal, chama PerderVida() que existe
                // Você precisa adicionar um método público para perder vida
                // Por enquanto, vou assumir que tem
                var metodo = playerScript.GetType().GetMethod("PerderVida");
                if (metodo != null) metodo.Invoke(playerScript, null);
            }
            else
            {
                // Tenta o PlayerFase2Real
                var playerFase2 = collision.gameObject.GetComponent<PlayerFase2Real>();
                if (playerFase2 != null)
                    playerFase2.PerderVida();
            }
        }
    }
}