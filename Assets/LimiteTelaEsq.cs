using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LimiteTelaEsq : MonoBehaviour
{
    [Header("Cena Destino")]
    public string nomeCenaDestino = "cutSceneFase3";

    [Header("Mensagem de Erro (UI Text)")]
    public GameObject mensagemErro; // deve começar DESATIVADO na Hierarchy

    private bool jaMostrouMensagem = false; // evita spam de mensagens

    void Start()
    {
        if (mensagemErro != null)
            mensagemErro.SetActive(false);   // garante que a mensagem comece desligada
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (InventoryManager.Instance != null && InventoryManager.Instance.pocao > 0)
            {
                Debug.Log("Player tem poção. Carregando cena: " + nomeCenaDestino);
                SceneManager.LoadScene(nomeCenaDestino);
            }
            else
            {
                if (!jaMostrouMensagem)
                {
                    StartCoroutine(MostrarMensagemTemporaria());
                }
            }
        }
    }

    IEnumerator MostrarMensagemTemporaria()
    {
        jaMostrouMensagem = true;
        if (mensagemErro != null)
        {
            mensagemErro.SetActive(true);
            yield return new WaitForSeconds(2f); // exibe por 2 segundos
            mensagemErro.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        jaMostrouMensagem = false; // permite nova mensagem após 1 segundo
    }
}