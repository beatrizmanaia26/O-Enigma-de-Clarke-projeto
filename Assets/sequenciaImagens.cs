using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SequenciaImagensComFade : MonoBehaviour
{
    public Image[] imagens;
    public float tempoFade = 0.5f;
    public string nomeProximaCena = "fase1Jogo";
    
    private int imagemAtual = 0;
    private bool podeAvancar = true;

    void Start()
    {
        Debug.Log("START - Total de imagens: " + imagens.Length);
        
        // Primeiro: DESATIVA todas as imagens (em vez de usar alpha)
        for (int i = 0; i < imagens.Length; i++)
        {
            if (imagens[i] != null)
            {
                imagens[i].gameObject.SetActive(false);
                Debug.Log("Imagem " + i + " desativada: " + imagens[i].name);
            }
        }
        
        // ATIVA a primeira imagem
        if (imagens[0] != null)
        {
            imagens[0].gameObject.SetActive(true);
            Debug.Log("Primeira imagem ATIVADA: " + imagens[0].name);
            
            // Garante que o alpha está 1
            Color cor = imagens[0].color;
            cor.a = 1;
            imagens[0].color = cor;
            Debug.Log("Alpha da primeira imagem setado para: " + imagens[0].color.a);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("ENTER pressionado!");
            AvancarImagem();
        }
    }

    void AvancarImagem()
    {
        if (!podeAvancar) return;
        
        Debug.Log("Avançando da imagem " + imagemAtual);
        
        if (imagemAtual + 1 < imagens.Length)
        {
            // Esconde imagem atual
            if (imagens[imagemAtual] != null)
                imagens[imagemAtual].gameObject.SetActive(false);
            
            // Avança
            imagemAtual++;
            
            // Mostra próxima imagem
            if (imagens[imagemAtual] != null)
            {
                imagens[imagemAtual].gameObject.SetActive(true);
                
                // Garante alpha 1
                Color cor = imagens[imagemAtual].color;
                cor.a = 1;
                imagens[imagemAtual].color = cor;
                
                Debug.Log("Mostrando imagem " + imagemAtual + ": " + imagens[imagemAtual].name);
            }
        }
        else
        {
            // Fim da sequência
            Debug.Log("Fim da sequência! Carregando: " + nomeProximaCena);
            SceneManager.LoadScene(nomeProximaCena);
        }
    }
}