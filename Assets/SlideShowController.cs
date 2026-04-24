using UnityEngine;
using UnityEngine.InputSystem;

public class SlideShowController : MonoBehaviour
{
    public GameObject[] imagens;
    private int indiceImagemAtual = 0;
    private bool slideshowAtivo = true;

    void Awake()
    {
        // Executa antes do Start, garante que tudo está pronto
        Debug.Log("Awake executado");
    }

    void Start()
    {
        Debug.Log("Start executado");
        
        // Verifica se tem imagens
        if (imagens == null || imagens.Length == 0)
        {
            Debug.LogError("❌ NENHUMA IMAGEM ATRIBUÍDA! Arraste as imagens do Canvas para o array no Inspector.");
            return;
        }
        
        Debug.Log($"✅ Total de imagens no array: {imagens.Length}");
        
        // Mostra todas as imagens que foram atribuídas
        for(int i = 0; i < imagens.Length; i++)
        {
            if (imagens[i] == null)
                Debug.LogError($"❌ Imagem na posição {i} é NULL! Arraste a imagem correta.");
            else
                Debug.Log($"✅ Imagem {i}: {imagens[i].name}");
        }
        
        // Desativa todas as imagens
        DesativarTodasImagens();
        
        // Mostra a primeira imagem
        MostrarImagemAtual();
    }

    void Update()
    {
        // Verifica ENTER
        if (slideshowAtivo && Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            AvancarParaProximaImagem();
        }
    }

    void AvancarParaProximaImagem()
    {
        if (imagens.Length == 0 || indiceImagemAtual >= imagens.Length)
            return;
            
        // Esconde a imagem atual
        if (imagens[indiceImagemAtual] != null)
        {
            imagens[indiceImagemAtual].SetActive(false);
            Debug.Log($"Escondendo imagem {indiceImagemAtual + 1}");
        }

        indiceImagemAtual++;

        if (indiceImagemAtual < imagens.Length)
        {
            MostrarImagemAtual();
        }
        else
        {
            slideshowAtivo = false;
            Debug.Log("📌 Fim do slideshow! Todas as 5 imagens foram mostradas.");
        }
    }

    void MostrarImagemAtual()
    {
        if (indiceImagemAtual < imagens.Length && imagens[indiceImagemAtual] != null)
        {
            imagens[indiceImagemAtual].SetActive(true);
            Debug.Log($"🎬 Mostrando imagem {indiceImagemAtual + 1}/{imagens.Length}");
        }
    }
    
    void DesativarTodasImagens()
    {
        foreach (GameObject img in imagens)
        {
            if (img != null)
                img.SetActive(false);
        }
        Debug.Log("Todas as imagens foram desativadas");
    }
}