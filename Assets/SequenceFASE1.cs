using UnityEngine;
using UnityEngine.SceneManagement;

public class SequenceFASE1 : MonoBehaviour
{
    [Header("Ordem correta")]
    public PuzzleItensFase1.PuzzleItemType[] correctSequence =
    {
        PuzzleItensFase1.PuzzleItemType.Tocha,
        PuzzleItensFase1.PuzzleItemType.Espada,
        PuzzleItensFase1.PuzzleItemType.Coroa
    };

    [Header("Botões do puzzle")]
    public PuzzleItensFase1 tochaButton;
    public PuzzleItensFase1 espadaButton;
    public PuzzleItensFase1 coroaButton;

    [Header("Vida da Clarke")]
    public PlayerFase2 player;

    [Header("Cena ao completar")]
    public string sceneToLoad = "fase2TelaInicial";

    [Header("Configuração")]
    public bool resetOnWrongClick = true;

    private int currentIndex = 0;
    private bool puzzleCompleted = false;

    private void Start()
    {
        ResetPuzzle();
    }

    public void ClickItem(PuzzleItensFase1 clickedButton)
    {
        if (puzzleCompleted) return;

        PuzzleItensFase1.PuzzleItemType expectedItem = correctSequence[currentIndex];

        if (clickedButton.itemType == expectedItem)
        {
            clickedButton.SetHighlighted(true);
            currentIndex++;

            Debug.Log("Acertou: " + clickedButton.itemType);

            if (currentIndex >= correctSequence.Length)
            {
                CompletePuzzle();
            }
        }
        else
        {
            ErrarPuzzle(clickedButton);
        }
    }

    private void ErrarPuzzle(PuzzleItensFase1 clickedButton)
    {
        Debug.Log("Errou! Clicou em: " + clickedButton.itemType);

        if (player != null)
        {
            player.PerderVida();
        }
        else
        {
            Debug.LogWarning("Player não foi ligado no SequenceFASE1.");
        }

        if (resetOnWrongClick)
        {
            ResetPuzzle();
        }
    }

    private void CompletePuzzle()
    {
        puzzleCompleted = true;

        Debug.Log("Puzzle completo! Indo para a cena: " + sceneToLoad);

        SceneManager.LoadScene(sceneToLoad);
    }

    public void ResetPuzzle()
    {
        currentIndex = 0;
        puzzleCompleted = false;

        if (tochaButton != null)
            tochaButton.SetHighlighted(false);

        if (espadaButton != null)
            espadaButton.SetHighlighted(false);

        if (coroaButton != null)
            coroaButton.SetHighlighted(false);
    }
}