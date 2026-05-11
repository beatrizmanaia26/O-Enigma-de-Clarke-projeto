using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("Mensagem 3D no mundo")]
    public GameObject mensagemObject;
    public TextMeshPro mensagemTexto;
    public float tempoMensagem = 2.5f;

    [Header("Configuração")]
    public bool resetOnWrongClick = true;

    private int currentIndex = 0;
    private bool puzzleCompleted = false;

    private void Start()
    {
        ResetPuzzle();

        if (mensagemObject != null)
            mensagemObject.SetActive(false);

        if (mensagemTexto != null)
            mensagemTexto.text = "";
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
                VerificarItensAntesDeCompletar();
            }
        }
        else
        {
            ErrarPuzzle(clickedButton);
        }
    }

    private void VerificarItensAntesDeCompletar()
    {
        if (InventoryManager.Instance == null)
        {
            MostrarMensagem("Inventário não encontrado.");
            ResetPuzzle();
            return;
        }

        string itensFaltando = "";

        if (InventoryManager.Instance.torches <= 0)
        {
            itensFaltando += "tocha";
        }

        if (!InventoryManager.Instance.hasSword)
        {
            if (itensFaltando != "") itensFaltando += ", ";
            itensFaltando += "espada";
        }

        if (InventoryManager.Instance.crowns <= 0)
        {
            if (itensFaltando != "") itensFaltando += ", ";
            itensFaltando += "coroa";
        }

        if (itensFaltando != "")
        {
            MostrarMensagem("Falta coletar: " + itensFaltando + ".");

            Debug.Log("Ordem correta, mas faltam itens: " + itensFaltando);

            // Acertou a ordem, mas faltou item:
            // não perde vida, só reseta os botões.
            ResetPuzzle();
            return;
        }

        CompletePuzzle();
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

        MostrarMensagem("Enigma resolvido!");

        Debug.Log("Puzzle completo! Todos os itens foram coletados. Indo para: " + sceneToLoad);

        Invoke(nameof(CarregarCenaFinal), 1f);
    }

    private void CarregarCenaFinal()
    {
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

    private void MostrarMensagem(string texto)
    {
        if (mensagemTexto != null)
            mensagemTexto.text = texto;

        if (mensagemObject != null)
        {
            mensagemObject.SetActive(true);

            CancelInvoke(nameof(EsconderMensagem));
            Invoke(nameof(EsconderMensagem), tempoMensagem);
        }

        Debug.Log(texto);
    }

    private void EsconderMensagem()
    {
        if (mensagemObject != null)
            mensagemObject.SetActive(false);
    }
}