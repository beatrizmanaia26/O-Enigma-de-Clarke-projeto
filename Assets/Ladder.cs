using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Destino (mesma cena)")]
    public Transform pontoDestino;       // Empty GameObject para onde o player vai
    
    [Header("Destino (outra cena)")]
    public string nomeCenaDestino = "";  // preencha se quiser ir para outra cena
    
    [Header("Identificação (opcional)")]
    public string escadaID = "escada1";  // só para debug
}