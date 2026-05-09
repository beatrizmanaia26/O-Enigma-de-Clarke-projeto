using UnityEngine;

public class Alavanca : MonoBehaviour
{
    public string tipoAlavanca;

    public PuzzleAlavancas puzzle;

    private bool playerPerto = false;

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            puzzle.RegistrarAlavanca(tipoAlavanca);

            Debug.Log("Puxou: " + tipoAlavanca);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
        }
    }
}