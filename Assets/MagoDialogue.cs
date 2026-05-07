// using UnityEngine;
// using TMPro; // se usar TextMeshPro; remova essa linha e use UnityEngine.UI.Text se preferir UI Text
// using System.Collections;

// public class MagoDialogue : MonoBehaviour
// {
//     [Header("Configuração do Diálogo")]
//     public string[] falas = { "Olá, viajante!", "O que te traz aqui?", "Cuidado com as aranhas!" };
//     public float velocidadeDigitacao = 0.05f; // tempo entre cada letra

//     [Header("UI")]
//     public GameObject dialogoPanel;          // o painel que contém o texto (começa desativado)
//     public TextMeshProUGUI dialogoTexto;     // componente de texto (se usar Text normal, troque para 'public Text dialogoTexto')

//     private bool playerPerto = false;
//     private bool estaFalando = false;

//     void Start()
//     {
//         if (dialogoPanel != null)
//             dialogoPanel.SetActive(false);
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//             playerPerto = true;
//     }

//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             playerPerto = false;
//             // Se estiver falando, cancela e esconde o painel
//             if (estaFalando)
//             {
//                 StopAllCoroutines();
//                 dialogoPanel.SetActive(false);
//                 estaFalando = false;
//             }
//         }
//     }

//     void Update()
//     {
//         if (playerPerto && Input.GetKeyDown(KeyCode.E) && !estaFalando)
//         {
//             StartCoroutine(MostrarDialogo());
//         }
//     }

//     IEnumerator MostrarDialogo()
//     {
//         estaFalando = true;
//         dialogoPanel.SetActive(true);

//         for (int i = 0; i < falas.Length; i++)
//         {
//             yield return StartCoroutine(DigitarFala(falas[i]));
//             yield return new WaitForSeconds(1f); // tempo entre falas
//         }

//         dialogoPanel.SetActive(false);
//         estaFalando = false;
//     }

//     IEnumerator DigitarFala(string frase)
//     {
//         dialogoTexto.text = "";
//         foreach (char letra in frase.ToCharArray())
//         {
//             dialogoTexto.text += letra;
//             yield return new WaitForSeconds(velocidadeDigitacao);
//         }
//     }
// }
using UnityEngine;
using TMPro;

public class MagoDialogue : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactHint;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Falas")]
    [TextArea(2, 5)]
    public string[] lines;

    private bool playerInRange = false;
    private bool dialogueOpen = false;
    private int currentLine = 0;

    private void Start()
    {
        if (interactHint != null)
            interactHint.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueOpen)
            {
                StartDialogue();
            }
            else
            {
                NextLine();
            }
        }
    }

    private void StartDialogue()
    {
        dialogueOpen = true;
        currentLine = 0;

        if (interactHint != null)
            interactHint.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null && lines.Length > 0)
            dialogueText.text = lines[currentLine];
    }

    private void NextLine()
    {
        currentLine++;

        if (currentLine < lines.Length)
        {
            dialogueText.text = lines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueOpen = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (playerInRange && interactHint != null)
            interactHint.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!dialogueOpen && interactHint != null)
                interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactHint != null)
                interactHint.SetActive(false);

            if (dialogueOpen)
                EndDialogue();
        }
    }
}