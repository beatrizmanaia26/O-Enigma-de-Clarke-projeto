using UnityEngine;

public class PuzzleItensFase1 : MonoBehaviour
{
    public enum PuzzleItemType
    {
        Tocha,
        Espada,
        Coroa
    }

    [Header("Tipo deste botão")]
    public PuzzleItemType itemType;

    [Header("Referência do puzzle")]
    public SequenceFASE1 puzzleManager;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.85f, 0.25f, 1f);

    [Header("Luz / destaque opcional")]
    public GameObject highlightObject;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        SetHighlighted(false);
    }

    private void OnMouseDown()
    {
        if (puzzleManager != null)
        {
            puzzleManager.ClickItem(this);
        }
        else
        {
            Debug.LogWarning("Puzzle Manager não foi ligado no botão: " + gameObject.name);
        }
    }

    public void SetHighlighted(bool highlighted)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlighted ? selectedColor : normalColor;
        }

        if (highlightObject != null)
        {
            highlightObject.SetActive(highlighted);
        }
    }
}