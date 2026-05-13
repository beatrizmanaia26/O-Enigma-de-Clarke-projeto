using UnityEngine;
using System.Collections;
using TMPro;  // ← ADICIONE ESTA LINHA

public class MostrarMensagem3D : MonoBehaviour
{
    [Header("Configuração")]
    public float tempoVisivel = 2f;  // Tempo que o texto fica visível
    public string mensagem = "✨ Poção Utilizada! ✨";

    private TextMeshPro texto3D;
    private Coroutine coroutineAtual;

    void Start()
    {
        // Pega o componente TextMeshPro do objeto
        texto3D = GetComponent<TextMeshPro>();
        
        if (texto3D == null)
        {
            Debug.LogWarning("MostrarMensagem3D: Não foi encontrado componente TextMeshPro!");
            return;
        }

        // Começa invisível
        texto3D.gameObject.SetActive(false);
    }

    public void MostrarMensagem()
    {
        if (texto3D == null) return;

        // Se já tem uma coroutine rodando, para ela
        if (coroutineAtual != null)
            StopCoroutine(coroutineAtual);

        // Configura o texto e mostra
        texto3D.text = mensagem;
        texto3D.gameObject.SetActive(true);

        // Inicia a coroutine para esconder depois
        coroutineAtual = StartCoroutine(EsconderAposTempo());
    }

    public void MostrarMensagemPersonalizada(string msg)
    {
        if (texto3D == null) return;

        if (coroutineAtual != null)
            StopCoroutine(coroutineAtual);

        texto3D.text = msg;
        texto3D.gameObject.SetActive(true);
        coroutineAtual = StartCoroutine(EsconderAposTempo());
    }

    IEnumerator EsconderAposTempo()
    {
        yield return new WaitForSeconds(tempoVisivel);
        
        if (texto3D != null)
            texto3D.gameObject.SetActive(false);
        
        coroutineAtual = null;
    }
}