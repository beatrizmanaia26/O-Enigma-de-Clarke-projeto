// using UnityEngine;

// public class TorchPickup : MonoBehaviour
// {
//     public GameObject torchHint;

//     private void Start()
//     {
//         if (torchHint != null)
//             torchHint.SetActive(false);
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             if (torchHint != null)
//                 torchHint.SetActive(true);
//         }
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             if (torchHint != null)
//                 torchHint.SetActive(false);
//         }
//     }

//     private void OnTriggerStay2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
//         {
//             if (InventoryManager.Instance != null)
//                 InventoryManager.Instance.AddTorch();

//             if (torchHint != null)
//                 torchHint.SetActive(false);

//             Destroy(gameObject);
//         }
//     }
// }