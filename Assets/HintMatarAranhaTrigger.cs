using UnityEngine;
using TMPro;

public class HintMatarAranhaTrigger : MonoBehaviour
{
    public GameObject hintTextObject;
    private TextMeshProUGUI hintText;

    private void Start()
    {
        if (hintTextObject != null)
        {
            hintText = hintTextObject.GetComponent<TextMeshProUGUI>();
            hintTextObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CompareTag("aranha"))
        {
            if (hintText != null)
                hintText.text = "Pressione E para matar a aranha!"; // Texto personalizado
            if (hintTextObject != null)
                hintTextObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CompareTag("aranha"))
        {
            if (hintTextObject != null)
                hintTextObject.SetActive(false);
        }
    }
}