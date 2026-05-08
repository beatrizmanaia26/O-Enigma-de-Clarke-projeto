// using UnityEngine;

// public class CrownPickup : MonoBehaviour
// {
//     public GameObject crownHint;

//     private void Start()
//     {
//         if (crownHint != null)
//             crownHint.SetActive(false);
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             if (crownHint != null)
//                 crownHint.SetActive(true);
//         }
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             if (crownHint != null)
//                 crownHint.SetActive(false);
//         }
//     }

//     private void OnTriggerStay2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
//         {
//             if (InventoryManager.Instance != null)
//                 InventoryManager.Instance.AddCrown();

//             if (crownHint != null)
//                 crownHint.SetActive(false);

//             Destroy(gameObject);
//         }
//     }
// }
using UnityEngine;

public class CrownPickup : MonoBehaviour
{
    public int value = 1;
    private bool playerProximo = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = true;
            Debug.Log("Perto da coroa! Aperte E para coletar");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximo = false;
        }
    }

    private void Update()
    {
        if (playerProximo && Input.GetKeyDown(KeyCode.E))
        {
            Coletar();
        }
    }

    private void Coletar()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddCrown(value);
            Debug.Log($"Coroa coletada! Total: {InventoryManager.Instance.crowns}");
        }
        Destroy(gameObject);
    }
}