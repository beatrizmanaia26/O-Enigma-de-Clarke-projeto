using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [Header("Texto da dica")]
    public GameObject hintObject;

    private void Start()
    {
        if (hintObject != null)
            hintObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintObject != null)
                hintObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintObject != null)
                hintObject.SetActive(false);
        }
    }
}