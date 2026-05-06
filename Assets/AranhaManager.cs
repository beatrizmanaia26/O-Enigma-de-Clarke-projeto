using UnityEngine;
using System.Collections.Generic;

public class AranhaManager : MonoBehaviour
{
    public static AranhaManager Instance;

    private List<Aranha> aranhas = new List<Aranha>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegistrarAranha(Aranha aranha)
    {
        if (!aranhas.Contains(aranha))
            aranhas.Add(aranha);
    }

    public void RemoverAranha(Aranha aranha)
    {
        if (aranhas.Contains(aranha))
            aranhas.Remove(aranha);
    }

    public void AoMatarAranha()
    {
        if (aranhas.Count == 0) return;

        int efeito = Random.Range(0, 2);
        Debug.Log("Efeito sorteado: " + (efeito == 0 ? "Aumentar Velocidade" : "Tornar Resistente"));

        foreach (Aranha aranha in aranhas)
        {
            if (efeito == 0)
                aranha.AumentarVelocidade();
            else
                aranha.TornarResistente();
        }
    }
}