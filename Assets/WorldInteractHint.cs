using UnityEngine;

public class WorldInteractHint : MonoBehaviour
{
    [Header("Objeto da dica no mundo")]
    public GameObject hintObject;

    private void Start()
    {
        if (hintObject != null)
            hintObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hintObject != null)
                hintObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (hintObject != null)
                hintObject.SetActive(false);
        }
    }
}