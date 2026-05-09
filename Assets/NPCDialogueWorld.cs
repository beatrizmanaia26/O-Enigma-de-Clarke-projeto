using UnityEngine;
using TMPro;

public class NPCDialogueWorld : MonoBehaviour
{
    [Header("Objetos no mundo")]
    public GameObject interactHint;
    public GameObject dialogueObject;
    public TextMeshPro dialogueText;

    [Header("Falas")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Tecla")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Depois do diálogo")]
    public GameObject[] objectsToActivateAfterDialogue;

    private bool playerIsNear = false;
    private bool dialogueStarted = false;
    private int currentLineIndex = 0;

    private void Start()
    {
        if (interactHint != null)
            interactHint.SetActive(false);

        if (dialogueObject != null)
            dialogueObject.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";
    }

    private void Update()
    {
        if (!playerIsNear) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (!dialogueStarted)
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
        dialogueStarted = true;
        currentLineIndex = 0;

        if (interactHint != null)
            interactHint.SetActive(false);

        if (dialogueObject != null)
            dialogueObject.SetActive(true);

        ShowCurrentLine();
    }

    private void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < lines.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowCurrentLine()
    {
        if (dialogueText != null)
            dialogueText.text = lines[currentLineIndex];
    }

    private void EndDialogue()
    {
        dialogueStarted = false;
        currentLineIndex = 0;

        if (dialogueObject != null)
            dialogueObject.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = "";

        foreach (GameObject obj in objectsToActivateAfterDialogue)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        Debug.Log("Diálogo do soldado finalizado.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;

            if (!dialogueStarted && interactHint != null)
                interactHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;

            if (interactHint != null)
                interactHint.SetActive(false);

            if (dialogueObject != null)
                dialogueObject.SetActive(false);

            if (dialogueText != null)
                dialogueText.text = "";

            dialogueStarted = false;
            currentLineIndex = 0;
        }
    }
}