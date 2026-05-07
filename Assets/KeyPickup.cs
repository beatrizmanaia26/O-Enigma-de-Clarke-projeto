// using UnityEngine;

// public class KeyPickup : MonoBehaviour
// {
//     public GameObject keyHint;

//     private void Start()
//     {
//         if (keyHint != null)
//             keyHint.SetActive(false);
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             if (keyHint != null)
//                 keyHint.SetActive(true);
//         }
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             if (keyHint != null)
//                 keyHint.SetActive(false);
//         }
//     }

//     private void OnTriggerStay2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
//         {
//             if (InventoryManager.Instance != null)
//                 InventoryManager.Instance.AddKey();

//             if (keyHint != null)
//                 keyHint.SetActive(false);

//             Destroy(gameObject);
//         }
//     }
// }