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

    [Header("Mensagens (personalizadas)")]
    public string mensagemSemChave = "❌ Não dá para passar sem a chave";
    public string mensagemSemCoroa = "❌ Não dá para passar sem a coroa";
    public string mensagemSemAmbos = "❌ Não dá para passar sem a coroa e a chave";
    public float tempoMensagem = 2f;
    public TextMeshProUGUI textoMensagemUI;

    [Header("Barreira Física (obrigatório)")]
    public GameObject barreiraFisica;          // objeto com Collider2D sólido
    public bool desativarBarreiraAposUso = true;
    public bool esconderSpriteBarreira = true;

    [Header("Ações extras")]
    public GameObject objetoParaAtivar;
    public string nomeProximaCena = "";
    public bool consumirItens = false;
    public bool destruirAposUsar = true;       // ← variável que estava faltando

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
            if (precisaCoroa) temCoroa = InventoryManager.Instance.crowns > 0;
            if (precisaChave) temChave = InventoryManager.Instance.keys > 0;
        }
        else
        {
            Debug.LogWarning("InventoryManager não encontrado!");
            return;
        }

        bool faltaCoroa = precisaCoroa && !temCoroa;
        bool faltaChave = precisaChave && !temChave;

        if (faltaCoroa && faltaChave)
        {
            MostrarMensagem(mensagemSemAmbos);
            return;
        }
        if (faltaChave)
        {
            MostrarMensagem(mensagemSemChave);
            return;
        }
        if (faltaCoroa)
        {
            MostrarMensagem(mensagemSemCoroa);
            return;
        }

        // Tudo ok, liberar passagem
        Passar();
    }

    void Passar()
    {
        jaUsado = true;

        // Liberar barreira física
        if (desativarBarreiraAposUso && barreiraFisica != null)
        {
            Collider2D col = barreiraFisica.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            if (esconderSpriteBarreira)
            {
                SpriteRenderer sr = barreiraFisica.GetComponent<SpriteRenderer>();
                if (sr != null) sr.enabled = false;
            }

            Debug.Log("Barreira física desativada. Passagem liberada!");
        }

        // Consumir itens, se solicitado
        if (consumirItens && InventoryManager.Instance != null)
        {
            if (precisaCoroa) InventoryManager.Instance.SpendCrown(1);
            if (precisaChave) InventoryManager.Instance.SpendKey(1);
            Debug.Log("Itens consumidos.");
        }

        // Ativar objeto extra
        if (objetoParaAtivar != null)
            objetoParaAtivar.SetActive(true);

        // Carregar próxima cena
        if (!string.IsNullOrEmpty(nomeProximaCena))
            SceneManager.LoadScene(nomeProximaCena);

        // Destruir este trigger (a área de interação)
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