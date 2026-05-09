using UnityEngine;

public class PlayerVisualSwap : MonoBehaviour
{
    [Header("Visuais")]
    public GameObject clarkeSemEspada;
    public GameObject clarkeComEspada;

    private PlayerFase2 player;

    private void Awake()
    {
        player = GetComponent<PlayerFase2>();
    }

    private void Start()
    {
        AtualizarVisual(true);
    }

    private void Update()
    {
        if (player != null && player.IsCrouching)
            return;

        bool olhandoDireita = player == null || player.ViradoParaDireita;
        AtualizarVisual(olhandoDireita);
    }

    public void GetSword()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddSword();
        }

        bool olhandoDireita = player == null || player.ViradoParaDireita;
        AtualizarVisual(olhandoDireita);
    }

    public void EsconderVisuaisNormais()
    {
        if (clarkeSemEspada != null)
            clarkeSemEspada.SetActive(false);

        if (clarkeComEspada != null)
            clarkeComEspada.SetActive(false);
    }

    public void AtualizarVisual(bool olhandoDireita)
    {
        bool temEspada = InventoryManager.Instance != null && InventoryManager.Instance.hasSword;

        if (clarkeSemEspada != null)
            clarkeSemEspada.SetActive(!temEspada);

        if (clarkeComEspada != null)
            clarkeComEspada.SetActive(temEspada);

        AplicarFlip(clarkeSemEspada, olhandoDireita);
        AplicarFlip(clarkeComEspada, olhandoDireita);
    }

    private void AplicarFlip(GameObject visual, bool olhandoDireita)
    {
        if (visual == null) return;

        SpriteRenderer sr = visual.GetComponent<SpriteRenderer>();

        if (sr != null)
            sr.flipX = !olhandoDireita;
    }
}