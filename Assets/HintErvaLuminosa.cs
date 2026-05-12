using UnityEngine;

public class HintErvaLuminosa : MonoBehaviour
{
    public GameObject hint3D;

    private void Start()
    {
        if (hint3D != null)
            hint3D.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hint3D != null)
                hint3D.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (hint3D != null)
                hint3D.SetActive(false);
        }
    }
}