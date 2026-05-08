using UnityEngine;
using System.Collections;
using TMPro;

public class Caldeirao : MonoBehaviour
{
    [Header("UI da Poção")]
    public GameObject imagemPocao;

    [Header("UI da Mensagem de Erro")]
    public GameObject mensagemErro;
    public float tempoMensagem = 2f;

    [Header("Tempo da Poção")]
    public float tempoExibicao = 3f;

    private bool playerPerto = false;
    private bool jaAtivou = false;

    void Start()
    {
        if (imagemPocao != null) imagemPocao.SetActive(false);
        if (mensagemErro != null) mensagemErro.SetActive(false);
        Debug.Log("[Caldeirao] Start – pronto.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[Caldeirao] Trigger entrou com: {other.name} (tag: {other.tag})");
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
            Debug.Log("[Caldeirao] Player detectado dentro do trigger.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
            Debug.Log("[Caldeirao] Player saiu do trigger.");
        }
    }

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[Caldeirao] Tecla E pressionada enquanto player perto.");
            if (!jaAtivou)
                VerificarItens();
            else
                Debug.Log("[Caldeirao] Caldeirão já foi ativado (jaAtivou = true).");
        }
    }

    void VerificarItens()
    {
        Debug.Log("[Caldeirao] Verificando itens...");
        InventoryManager inv = InventoryManager.Instance;
        if (inv == null)
        {
            Debug.LogError("[Caldeirao] InventoryManager.Instance é NULL!");
            return;
        }

        bool temEstrela = inv.stars > 0;
        bool temPedra   = inv.blueStones > 0;
        bool temFlor    = inv.flor > 0;

        bool ordemCorreta = inv.ordemColeta.Count == 3 &&
                            inv.ordemColeta[0] == "estrela" &&
                            inv.ordemColeta[1] == "pedraAzul" &&
                            inv.ordemColeta[2] == "flor";

        Debug.Log($"[Caldeirao] Tem estrela: {temEstrela} | Tem pedra: {temPedra} | Tem flor: {temFlor}");
        Debug.Log($"[Caldeirao] Ordem atual: {string.Join(", ", inv.ordemColeta)}");
        Debug.Log($"[Caldeirao] Ordem correta? {ordemCorreta}");

        if (temEstrela && temPedra && temFlor && ordemCorreta)
        {
            Debug.Log("[Caldeirao] CONDICAO OK – vai mostrar poção.");
            StartCoroutine(MostrarPocao());
            jaAtivou = true;
        }
        else
        {
            Debug.Log("[Caldeirao] Condição falhou – vai mostrar mensagem de erro.");
            StartCoroutine(MostrarMensagemErro());
        }
    }

    IEnumerator MostrarPocao()
    {
        Debug.Log("[Caldeirao] MostrarPocao: ativando imagem.");
        if (imagemPocao != null)
        {
            imagemPocao.SetActive(true);
            yield return new WaitForSeconds(tempoExibicao);
            imagemPocao.SetActive(false);
            Debug.Log("[Caldeirao] Poção desativada após tempo.");
        }
        else
        {
            Debug.LogError("[Caldeirao] imagemPocao é NULL! Arraste o GameObject da poção no Inspector.");
        }
    }

    IEnumerator MostrarMensagemErro()
    {
        if (mensagemErro != null)
        {
            mensagemErro.SetActive(true);
            yield return new WaitForSeconds(tempoMensagem);
            mensagemErro.SetActive(false);
            Debug.Log("[Caldeirao] Mensagem de erro exibida.");
        }
        else
        {
            Debug.LogWarning("[Caldeirao] mensagemErro não atribuída.");
        }
    }
}