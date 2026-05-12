using UnityEngine;
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
        "Mas cuidado... a jornada ainda não terminou.",
        "A bruxa sabe que você chegou até aqui. A Torre Final aguarda seus passos."
    };

    [Header("UI")]
    public GameObject canvasDialogo;
    public TextMeshProUGUI textoDialogo;

    [Header("Digitação")]
    public float tempoPorLetra = 0.05f;
    public float tempoEntreFalas = 1f;

    [Header("Configuração")]
    public bool falarApenasUmaVez = true;

    private bool playerPerto = false;
    private bool dialogoAtivo = false;
    private bool jaFalou = false;

    void Start()
    {
        if (canvasDialogo != null)
            canvasDialogo.SetActive(false);
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

        if (canvasDialogo != null)
            canvasDialogo.SetActive(true);

        for (int i = 0; i < falas.Length; i++)
        {
            yield return StartCoroutine(DigitarFala(falas[i]));
            yield return new WaitForSeconds(tempoEntreFalas);
        }

        if (canvasDialogo != null)
            canvasDialogo.SetActive(false);

        if (textoDialogo != null)
            textoDialogo.text = "";

        dialogoAtivo = false;
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