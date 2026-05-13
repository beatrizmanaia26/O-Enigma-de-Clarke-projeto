using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class AranhaBauDialogue : MonoBehaviour
{
    [Header("Diálogo da Aranha do Baú")]
    [TextArea(3, 6)]
    public string[] falas = new string[]
    {
        "Oh... você conseguiu chegar até aqui, jovem Clarke.",
        "Eu sou a aranha que habita este antigo baú há muitos anos.",
        "Foi um caminho difícil, mas você resolveu todos os enigmas até aqui.",
        "O pergaminho que a bruxa roubou guarda mais do que simples pistas.",
        "Nele estão lendas antigas sobre os tesouros do reino, passagens perdidas e segredos que a realeza tentou esconder.",
        "Dizem que apenas alguém atento, corajoso e digno conseguiria compreender tudo o que está escrito nele.",
        "Você provou ser essa pessoa, Clarke.",
        "Agora, receba o pergaminho real.",
        "A jornada finalmente chegou ao fim.",
        "Que os antigos reis guiem seus passos, Clarke."
    };

    [Header("UI")]
    public GameObject canvasDialogo;
    public TextMeshProUGUI textoDialogo;

    [Header("Digitação")]
    public float tempoPorLetra = 0.05f;
    public float tempoEntreFalas = 1f;

    [Header("Cena de vitória")]
    public string nomeCenaVitoria = "telaVitoria";  // Nome exato da sua cena de vitória
    public float tempoAntesDeCarregarVitoria = 1f;  // Tempo de espera após o fim do diálogo

    [Header("Configuração")]
    public bool falarApenasUmaVez = true;

    private bool playerPerto = false;
    private bool dialogoAtivo = false;
    private bool jaFalou = false;

    void Start()
    {
        if (canvasDialogo != null)
            canvasDialogo.SetActive(false);

        if (textoDialogo != null)
            textoDialogo.text = "";
    }

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E) && !dialogoAtivo)
        {
            if (falarApenasUmaVez && jaFalou)
                return;

            StartCoroutine(IniciarDialogo());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
            Debug.Log("Clarke chegou perto da Aranha do Baú. Aperte E para conversar.");
            
            // Opcional: Mostrar um texto na tela "Aperte E para falar com a aranha"
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;

            if (dialogoAtivo)
            {
                StopAllCoroutines();

                if (canvasDialogo != null)
                    canvasDialogo.SetActive(false);

                if (textoDialogo != null)
                    textoDialogo.text = "";

                dialogoAtivo = false;
            }
        }
    }

    IEnumerator IniciarDialogo()
    {
        dialogoAtivo = true;
        jaFalou = true;

        // Pausar o jogo enquanto o diálogo acontece (opcional)
        // Time.timeScale = 0f;

        if (canvasDialogo != null)
            canvasDialogo.SetActive(true);

        for (int i = 0; i < falas.Length; i++)
        {
            yield return StartCoroutine(DigitarFala(falas[i]));
            yield return new WaitForSeconds(tempoEntreFalas);
        }

        // Fechar o diálogo
        if (canvasDialogo != null)
            canvasDialogo.SetActive(false);

        if (textoDialogo != null)
            textoDialogo.text = "";

        dialogoAtivo = false;

        // Restaurar o tempo do jogo se tiver pausado
        // Time.timeScale = 1f;

        // Aguardar um pouco e carregar a cena de vitória
        yield return new WaitForSeconds(tempoAntesDeCarregarVitoria);

        Debug.Log("Diálogo finalizado! Carregando tela de vitória...");
        SceneManager.LoadScene(nomeCenaVitoria);
    }

    IEnumerator DigitarFala(string frase)
    {
        if (textoDialogo == null)
            yield break;

        textoDialogo.text = "";

        foreach (char letra in frase)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(tempoPorLetra);
        }
    }
}