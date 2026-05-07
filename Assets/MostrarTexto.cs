using UnityEngine;

public class MostrarTexto : MonoBehaviour
{
    public GameObject texto;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (texto != null)
            {
                texto.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (texto != null)
            {
                texto.SetActive(false);
            }
        }
    }
}