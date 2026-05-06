using UnityEngine;
using TMPro;

public class NPCDialogue1 : MonoBehaviour
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