using UnityEngine;
using TMPro;
using System.Collections;

public class MagoDialogue : MonoBehaviour
{
    [Header("Diálogo")]
    [TextArea(3,5)]
    public string[] falas = new string[]
    {
        "Olá, eu sou o mago. Como posso ajudá-la?",
        "Como faço para derrotar a bruxa?",
        "Para enfraquecê-la você precisa criar uma poção.",
        "Siga o enigma:\n\nPrimeiro, o sangue estrelado que brota sem feridas.\n\nDepois, a lágrima fria da pedra.\n\nPor fim, a raiz azul que atravessa a escuridão.",
        "Colete os ingredientes nessa ordem e depois use o caldeirão para criar a poção."
    };

    [Header("UI")]
    public GameObject canvasDialogo;
    public TextMeshProUGUI textoDialogo;

    [Header("Digitação")]
    public float tempoPorLetra = 0.05f;
    public float tempoEntreFalas = 1f;

    private bool playerPerto = false;
    private bool dialogoAtivo = false;

    void Start()
    {
        if (canvasDialogo != null)
            canvasDialogo.SetActive(false);
        
        // Opcional: log para verificar as falas carregadas
        for (int i = 0; i < falas.Length; i++)
            Debug.Log($"Fala {i}: {falas[i]}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerPerto = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
            if (dialogoAtivo)
            {
                StopAllCoroutines();
                canvasDialogo.SetActive(false);
                dialogoAtivo = false;
            }
        }
    }

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E) && !dialogoAtivo)
        {
            StartCoroutine(IniciarDialogo());
        }
    }

    IEnumerator IniciarDialogo()
    {
        dialogoAtivo = true;
        canvasDialogo.SetActive(true);

        for (int i = 0; i < falas.Length; i++)
        {
            yield return StartCoroutine(DigitarFala(falas[i]));
            yield return new WaitForSeconds(tempoEntreFalas);
        }

        canvasDialogo.SetActive(false);
        dialogoAtivo = false;
    }

    IEnumerator DigitarFala(string frase)
    {
        textoDialogo.text = "";
        foreach (char letra in frase)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(tempoPorLetra);
        }
    }
}