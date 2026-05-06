using UnityEngine;

public class PlayerVisualSwap : MonoBehaviour
{
    [SerializeField] private GameObject clarkeSemEspada;
    [SerializeField] private GameObject clarkeComEspada;

    private void Start()
    {
        UpdateVisual();
    }

    public void GetSword()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddSword();
        }

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        bool hasSword = InventoryManager.Instance != null && InventoryManager.Instance.hasSword;

        if (clarkeSemEspada != null)
            clarkeSemEspada.SetActive(!hasSword);

        if (clarkeComEspada != null)
            clarkeComEspada.SetActive(hasSword);
    }
}