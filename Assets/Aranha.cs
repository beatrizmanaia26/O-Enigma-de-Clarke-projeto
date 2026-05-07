// using UnityEngine;
// using System.Collections;

// public class Aranha : MonoBehaviour
// {
//     [Header("Movimento")]
//     public float velocidadeBase = 2f;
//     public Transform limiteEsquerda;
//     public Transform limiteDireita;

//     private float velocidadeAtual;
//     private bool movendoParaDireita = true;
//     private Rigidbody2D rb;
//     private SpriteRenderer sr;
//     private int vida = 1;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         sr = GetComponent<SpriteRenderer>();
//         if (rb == null)
//         {
//             rb = gameObject.AddComponent<Rigidbody2D>();
//             rb.bodyType = RigidbodyType2D.Kinematic;
//         }
//         velocidadeAtual = velocidadeBase;

//         if (AranhaManager.Instance != null)
//             AranhaManager.Instance.RegistrarAranha(this);
//         else
//             Debug.LogWarning("AranhaManager não encontrado na cena!");
//     }

//     void Update()
//     {
//         Mover();
//     }

//     void Mover()
//     {
//         if (limiteEsquerda == null || limiteDireita == null) return;
//         float direcao = movendoParaDireita ? 1f : -1f;
//         rb.linearVelocity = new Vector2(direcao * velocidadeAtual, rb.linearVelocity.y);

//         if (movendoParaDireita && transform.position.x >= limiteDireita.position.x)
//         {
//             movendoParaDireita = false;
//             Virar();
//         }
//         else if (!movendoParaDireita && transform.position.x <= limiteEsquerda.position.x)
//         {
//             movendoParaDireita = true;
//             Virar();
//         }
//     }

//     void Virar()
//     {
//         Vector3 escala = transform.localScale;
//         escala.x *= -1;
//         transform.localScale = escala;
//     }

//     public void ReceberDano()
//     {
//         vida--;
//         if (vida <= 0)
//             Morrer();
//         else
//             StartCoroutine(Piscar());
//     }

//     void Morrer()
//     {
//         if (AranhaManager.Instance != null)
//         {
//             AranhaManager.Instance.RemoverAranha(this);
//             AranhaManager.Instance.AoMatarAranha();
//         }
//         Destroy(gameObject);
//     }

//     public void AumentarVelocidade()
//     {
//         velocidadeAtual = velocidadeBase * 1.5f;
//         if (sr != null)
//             sr.color = new Color(1f, 0.5f, 0.5f); // avermelhado
//         Debug.Log(name + " velocidade aumentada para " + velocidadeAtual);
//     }

//     public void TornarResistente()
//     {
//         vida = 2;
//         if (sr != null)
//             sr.color = new Color(0.5f, 0.5f, 1f); // azulado
//         Debug.Log(name + " agora é resistente (vida = 2)");
//     }

//     IEnumerator Piscar()
//     {
//         if (sr == null) yield break;
//         Color original = sr.color;
//         for (int i = 0; i < 3; i++)
//         {
//             sr.color = Color.white;
//             yield return new WaitForSeconds(0.1f);
//             sr.color = original;
//             yield return new WaitForSeconds(0.1f);
//         }
//     }
// }
using UnityEngine;
using System.Collections;

public class Aranha : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 2f;
    
    // Limites - você pode arrastar os objetos ou deixar vazio que ele procura
    public Transform limiteEsquerda;
    public Transform limiteDireita;
    
    private bool andandoParaDireita = true;
    private SpriteRenderer sr;
    private int vida = 1;
    private float limiteEsqX;
    private float limiteDirX;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        // Configurar limites
        ConfigurarLimites();
        
        // Registrar no manager
        if (AranhaManager.Instance != null)
            AranhaManager.Instance.RegistrarAranha(this);
    }
    
    void ConfigurarLimites()
    {
        // Se não foram arrastados, procura na cena
        if (limiteEsquerda == null)
        {
            GameObject esq = GameObject.Find("limiteAranha_esqLinha1");
            if (esq != null) limiteEsquerda = esq.transform;
        }
        
        if (limiteDireita == null)
        {
            GameObject dir = GameObject.Find("limiteAranha_dirLinha1");
            if (dir != null) limiteDireita = dir.transform;
        }
        
        // Se ainda não tem, usa limites baseados na posição atual
        if (limiteEsquerda == null || limiteDireita == null)
        {
            Debug.LogWarning($"{name}: Usando limites automáticos (distância 3)");
            limiteEsqX = transform.position.x - 3f;
            limiteDirX = transform.position.x + 3f;
        }
        else
        {
            limiteEsqX = limiteEsquerda.position.x;
            limiteDirX = limiteDireita.position.x;
            Debug.Log($"{name}: Limites configurados! Esq={limiteEsqX}, Dir={limiteDirX}");
        }
        
        // Garantir que a aranha comece dentro dos limites
        float xAranha = transform.position.x;
        if (xAranha < limiteEsqX) transform.position = new Vector3(limiteEsqX, transform.position.y);
        if (xAranha > limiteDirX) transform.position = new Vector3(limiteDirX, transform.position.y);
    }

    void Update()
    {
        Mover();
    }
    
    void Mover()
    {
        // Calcula nova posição
        float direcao = andandoParaDireita ? 1f : -1f;
        float novoX = transform.position.x + (direcao * velocidade * Time.deltaTime);
        
        // Verifica limites
        if (novoX >= limiteDirX)
        {
            novoX = limiteDirX;
            andandoParaDireita = false;
            Virar();
        }
        else if (novoX <= limiteEsqX)
        {
            novoX = limiteEsqX;
            andandoParaDireita = true;
            Virar();
        }
        
        // Aplica movimento
        transform.position = new Vector3(novoX, transform.position.y, transform.position.z);
    }
    
    void Virar()
    {
        Vector3 escala = transform.localScale;
        escala.x = andandoParaDireita ? Mathf.Abs(escala.x) : -Mathf.Abs(escala.x);
        transform.localScale = escala;
    }

    public void ReceberDano()
    {
        vida--;
        if (vida <= 0)
            Morrer();
        else
            StartCoroutine(Piscar());
    }

    void Morrer()
    {
        if (AranhaManager.Instance != null)
            AranhaManager.Instance.AoMatarAranha();
        Destroy(gameObject);
    }

    public void AumentarVelocidade()
    {
        velocidade *= 1.5f;
        if (sr != null) sr.color = Color.red;
    }

    public void TornarResistente()
    {
        vida = 2;
        if (sr != null) sr.color = Color.blue;
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