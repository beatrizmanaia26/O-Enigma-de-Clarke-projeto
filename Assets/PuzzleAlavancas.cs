using UnityEngine;
using UnityEngine.SceneManagement;


public class PuzzleAlavancas : MonoBehaviour
{
    public string[] ordemCorreta =
    {
        "shield",
        "sword",
        "crown"
    };

    private int indiceAtual = 0;

    public PlayerFase2Real player;

    public void RegistrarAlavanca(string tipo)
    {
        // ACERTOU
        if (tipo == ordemCorreta[indiceAtual])
        {
            Debug.Log("Alavanca correta!");

            indiceAtual++;

            // TERMINOU O PUZZLE
            if (indiceAtual >= ordemCorreta.Length)
            {
                PuzzleCompleto();
            }
        }
        else
        {
            Debug.Log("Sequência errada!");

            indiceAtual = 0;

            if (player != null)
            {
                player.PerderVida();
            }
        }
    }

    void PuzzleCompleto()
    {
        Debug.Log("Puzzle completo!");
        SceneManager.LoadScene("fase3TelaInicial");

    }
}