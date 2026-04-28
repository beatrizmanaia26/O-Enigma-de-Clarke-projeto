using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Para mudar de cena

public class TextoDigitado : MonoBehaviour
{
    [Header("Componentes")]
    public TMP_Text textoVisor;     // Seu Text (TMP)
    public Button botaoAvancar;     // Seu botão "botaoAvancar"
    
    [Header("Configurações")]
    public float velocidade = 0.05f;  // Velocidade da digitação
    public string nomeProximaCena = "novoJogoHistoria2";  // Nome da cena para mudar
    
    private string textoCompleto;
    private bool textoTerminado = false;  // Controle se já pode mudar de cena
    
    void Start()
    {
        // Salva o texto original
        textoCompleto = textoVisor.text;
        
        // Limpa o texto
        textoVisor.text = "";
        
        // Esconde o botão no começo
        if (botaoAvancar != null)
        {
            botaoAvancar.gameObject.SetActive(false);
        }
        
        // Começa a digitar
        StartCoroutine(DigitarLetraPorLetra());
    }
    
    void Update()
    {
        // Verifica se a tecla Enter foi pressionada
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Pode mudar de cena a qualquer momento
            MudarParaProximaCena();
        }
    }
    
    IEnumerator DigitarLetraPorLetra()
    {
        // Digita letra por letra
        for (int i = 0; i <= textoCompleto.Length; i++)
        {
            textoVisor.text = textoCompleto.Substring(0, i);
            yield return new WaitForSeconds(velocidade);
        }
        
        // Marca que o texto terminou
        textoTerminado = true;
        
        // Quando terminar de digitar, mostra o botão com fade
        if (botaoAvancar != null)
        {
            CanvasGroup cg = botaoAvancar.GetComponent<CanvasGroup>();
            
            botaoAvancar.gameObject.SetActive(true);
            
            if (cg != null)
            {
                cg.alpha = 0;
                float tempo = 0;
                while (tempo < 0.5f)
                {
                    tempo += Time.deltaTime;
                    cg.alpha = tempo / 0.5f;
                    yield return null;
                }
                cg.alpha = 1;
            }
        }
    }
    
    void MudarParaProximaCena()
    {
        // Carrega a próxima cena
        SceneManager.LoadScene(nomeProximaCena);
    }
}