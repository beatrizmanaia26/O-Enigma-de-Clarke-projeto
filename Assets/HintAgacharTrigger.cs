using UnityEngine;

public class HintAgacharTrigger : MonoBehaviour
{
    [Header("Texto 3D da dica")]
    public GameObject hintAgachar;

    private void Start()
    {
        if (hintAgachar != null)
            hintAgachar.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintAgachar != null)
                hintAgachar.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintAgachar != null)
                hintAgachar.SetActive(false);
        }
    }
}