using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnigmaFase4 : MonoBehaviour
{
    [Header("Interação")]
    public KeyCode teclaInteracao = KeyCode.E;
    
    [Header("Itens Necessários")]
    public bool precisaCoroa = true;
    public bool precisaChave = true;
    
    [Header("Mensagem")]
    [TextArea(2, 4)]
    public string mensagemFaltaCoroa = "❌ Não é possível passar sem a COROA!";
    [TextArea(2, 4)]
    public string mensagemFaltaChave = "❌ Não é possível passar sem a CHAVE!";
    [TextArea(2, 4)]
    public string mensagemFaltaAmbos = "❌ Não é possível passar sem a COROA e a CHAVE!";
    public float tempoMensagem = 2f;
    public TextMeshProUGUI textoMensagemUI;
    
    [Header("Ação ao passar")]
    public GameObject objetoParaAtivar;
    public string nomeProximaCena = "";
    public bool destruirAposUsar = true;
    public bool consumirItens = false; // Se verdadeiro, remove coroa e chave ao usar
    
    private bool jogadorProximo = false;
    private bool jaUsado = false;
    private Coroutine coroutineMensagem;
    
    void Start()
    {
        if (textoMensagemUI == null)
            textoMensagemUI = FindObjectOfType<TextMeshProUGUI>();
        
        if (textoMensagemUI != null)
            textoMensagemUI.gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (!jaUsado && jogadorProximo && Input.GetKeyDown(teclaInteracao))
        {
            VerificarEProsseguir();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            jogadorProximo = true;
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            jogadorProximo = false;
    }
    
    void VerificarEProsseguir()
    {
        bool temCoroa = false;
        bool temChave = false;
        
        if (InventoryManager.Instance != null)
        {
            if (precisaCoroa)
                temCoroa = InventoryManager.Instance.crowns > 0;
            if (precisaChave)
                temChave = InventoryManager.Instance.keys > 0;
        }
        else
        {
            Debug.LogWarning("InventoryManager não encontrado!");
            return;
        }
        
        bool podePassar = true;
        string mensagem = "";
        
        if (precisaCoroa && !temCoroa && precisaChave && !temChave)
        {
            podePassar = false;
            mensagem = mensagemFaltaAmbos;
        }
        else if (precisaCoroa && !temCoroa)
        {
            podePassar = false;
            mensagem = mensagemFaltaCoroa;
        }
        else if (precisaChave && !temChave)
        {
            podePassar = false;
            mensagem = mensagemFaltaChave;
        }
        
        if (podePassar)
        {
            Passar();
        }
        else
        {
            MostrarMensagem(mensagem);
        }
    }
    
    void Passar()
    {
        jaUsado = true;
        Debug.Log("EnigmaFase4: Jogador passou com os itens corretos!");
        
        if (objetoParaAtivar != null)
            objetoParaAtivar.SetActive(true);
        
        if (consumirItens && InventoryManager.Instance != null)
        {
            if (precisaCoroa) InventoryManager.Instance.SpendCrown(1);
            if (precisaChave) InventoryManager.Instance.SpendKey(1);
            Debug.Log("Itens consumidos: coroa e/ou chave.");
        }
        
        if (!string.IsNullOrEmpty(nomeProximaCena))
            SceneManager.LoadScene(nomeProximaCena);
        
        if (destruirAposUsar)
            Destroy(gameObject);
    }
    
    void MostrarMensagem(string msg)
    {
        if (textoMensagemUI == null)
        {
            Debug.LogWarning(msg);
            return;
        }
        
        if (coroutineMensagem != null)
            StopCoroutine(coroutineMensagem);
        
        textoMensagemUI.text = msg;
        textoMensagemUI.gameObject.SetActive(true);
        coroutineMensagem = StartCoroutine(DesativarMensagem(tempoMensagem));
    }
    
    IEnumerator DesativarMensagem(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        if (textoMensagemUI != null)
            textoMensagemUI.gameObject.SetActive(false);
        coroutineMensagem = null;
    }
}