using UnityEngine;

public class SwordPickup : MonoBehaviour
{
    public GameObject pickupHint;

    private void Start()
    {
        if (pickupHint != null)
            pickupHint.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entrou no trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entrou na área da espada");

            if (pickupHint != null)
                pickupHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Saiu do trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            if (pickupHint != null)
                pickupHint.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player dentro da área da espada");

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E apertado na espada");

                PlayerVisualSwap player = other.GetComponent<PlayerVisualSwap>();

                if (player == null)
                    player = other.GetComponentInParent<PlayerVisualSwap>();

                if (player != null)
                {
                    Debug.Log("Pegou a espada");
                    player.GetSword();

                    if (pickupHint != null)
                        pickupHint.SetActive(false);

                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("PlayerVisualSwap não encontrado");
                }
            }
        }
    }
}