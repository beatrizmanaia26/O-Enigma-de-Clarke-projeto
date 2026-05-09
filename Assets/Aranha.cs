using UnityEngine;
using System.Collections;

public class Aranha : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 2f;
    
    // Limites - você pode arrastar os objetos ou deixar vazio que ele procura
    public Transform limiteEsquerda;
    public Transform limiteDireita;
    
    [Header("Save System")]
    public string idInimigo = ""; // ID único para save
    
    private bool andandoParaDireita = true;
    private SpriteRenderer sr;
    private int vida = 1;
    private float limiteEsqX;
    private float limiteDirX;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        // Criar ID único se não tiver
        if (string.IsNullOrEmpty(idInimigo))
            idInimigo = $"aranha_{gameObject.name}_{transform.position.x}_{transform.position.y}";
        
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
        // REGISTRAR NO SISTEMA DE PROGRESSO
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SistemaProgresso progresso = player.GetComponent<SistemaProgresso>();
            if (progresso != null)
                progresso.DerrotarInimigo(idInimigo);
        }
        
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