using UnityEngine;

public class Livro : MonoBehaviour
{
    [Header("Aranhas")]
    public GameObject[] aranhas;  // Array para várias aranhas
    
    private bool jaAtivado = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !jaAtivado)
        {
            AtivarAranhas();
            jaAtivado = true;
        }
    }
    
    private void AtivarAranhas()
    {
        foreach (GameObject aranha in aranhas)
        {
            if (aranha != null)
            {
                aranha.SetActive(true);
            }
        }
        Debug.Log($"{aranhas.Length} aranhas ativadas!");
    }
}