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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerPerto = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerPerto = false;
    }

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E) && !jaAtivou)
        {
            VerificarItens();
        }
    }

    void VerificarItens()
    {
        InventoryManager inv = InventoryManager.Instance;
        if (inv == null) return;

        bool temEstrela = inv.stars > 0;
        bool temPedra  = inv.blueStones > 0;
        bool temFlor   = inv.flor > 0;

        // Verifica se a ordem de coleta é exatamente estrela -> pedra -> flor
        bool ordemCorreta = inv.ordemColeta.Count == 3 &&
                            inv.ordemColeta[0] == "estrela" &&
                            inv.ordemColeta[1] == "pedraAzul" &&
                            inv.ordemColeta[2] == "flor";

        if (temEstrela && temPedra && temFlor && ordemCorreta)
        {
            StartCoroutine(MostrarPocao());
            jaAtivou = true;
            // Opcional: consumir os itens
            // inv.stars--; inv.blueStones--; inv.flor--;
            // inv.ResetarOrdem(); // limpa a ordem após usar
            Debug.Log("Poção criada com sucesso na ordem correta!");
        }
        else
        {
            StartCoroutine(MostrarMensagemErro());
            Debug.Log("Faltam itens ou a ordem de coleta está errada. A ordem deve ser: estrela → pedra azul → flor.");
        }
    }

    IEnumerator MostrarPocao()
    {
        imagemPocao.SetActive(true);
        yield return new WaitForSeconds(tempoExibicao);
        imagemPocao.SetActive(false);
    }

    IEnumerator MostrarMensagemErro()
    {
        if (mensagemErro != null)
        {
            mensagemErro.SetActive(true);
            yield return new WaitForSeconds(tempoMensagem);
            mensagemErro.SetActive(false);
        }
    }
}