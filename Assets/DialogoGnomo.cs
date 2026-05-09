using UnityEngine;
using TMPro;
using System.Collections;

public class DialogoGnomo : MonoBehaviour
{
    [Header("Diálogo")]
    [TextArea(3,5)]
    public string[] falas;

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
        canvasDialogo.SetActive(false);
    }

    void Update()
    {
        if (playerPerto &&
            Input.GetKeyDown(KeyCode.E) &&
            !dialogoAtivo)
        {
            StartCoroutine(IniciarDialogo());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;

            StopAllCoroutines();

            canvasDialogo.SetActive(false);

            dialogoAtivo = false;
        }
    }

    IEnumerator IniciarDialogo()
    {
        dialogoAtivo = true;

        canvasDialogo.SetActive(true);

        for (int i = 0; i < falas.Length; i++)
        {
            yield return StartCoroutine(
                DigitarFala(falas[i])
            );

            yield return new WaitForSeconds(
                tempoEntreFalas
            );
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

            yield return new WaitForSeconds(
                tempoPorLetra
            );
        }
    }
}