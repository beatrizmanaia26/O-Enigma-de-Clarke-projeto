using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SistemaProgresso : MonoBehaviour
{
    public List<string> itensColetados = new List<string>();
    public List<string> inimigosMortos = new List<string>();
    public GameObject jogador;

    void Start()
    {
        Debug.Log($"SistemaProgresso iniciado na cena: {SceneManager.GetActiveScene().name}");
        // Se o campo jogador estiver vazio, tenta encontrar automaticamente
        if (jogador == null)
            jogador = GameObject.FindGameObjectWithTag("Player");
        
        StartCoroutine(RestaurarAposDelay());
    }

    IEnumerator RestaurarAposDelay()
    {
        yield return new WaitForSeconds(0.2f);
        string cenaAtual = SceneManager.GetActiveScene().name;

        if (GameStateManager.Instance != null && GameStateManager.Instance.ExisteEstadoSalvo(cenaAtual))
            RestaurarEstado();
        else
            Debug.Log($"Nenhum estado salvo para {cenaAtual}");
    }

    public void RestaurarEstado()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        var estado = GameStateManager.Instance.RecuperarEstadoFase(cenaAtual);
        if (estado == null) return;

        Debug.Log($"=== RESTAURANDO {cenaAtual} ===");

        // Restaurar posição e vida do jogador
        if (jogador != null)
        {
            var playerScript = jogador.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.SetPosition(estado.posicaoJogador);
                playerScript.VidasRestantes = (int)estado.vidaJogador;
                Debug.Log($"Posição: {estado.posicaoJogador}, Vidas: {estado.vidaJogador}");
            }
        }

        // Restaurar inventário
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.coins = estado.moedas;
            InventoryManager.Instance.stars = estado.estrelas;
            InventoryManager.Instance.blueStones = estado.pedrasAzuis;
            InventoryManager.Instance.flor = estado.flores;
        
            InventoryManager.Instance.ervaCura = estado.ervaCura;
            InventoryManager.Instance.pocao = estado.pocoes;
            InventoryManager.Instance.keys = estado.chaves;
            InventoryManager.Instance.torches = estado.tochas;
            InventoryManager.Instance.hasSword = estado.hasSword;
            Debug.Log($"Inventário restaurado: Moedas={estado.moedas}, Tochas={estado.tochas}");
        }

        // Restaurar listas
        itensColetados = new List<string>(estado.itensColetados);
        inimigosMortos = new List<string>(estado.inimigosMortos);

        Debug.Log($"Itens coletados: {itensColetados.Count}");
        foreach (string item in itensColetados) Debug.Log($"  - {item}");

        // Remover itens e inimigos
        RemoverItensColetados();
        RemoverInimigosMortos();
    }

    void RemoverItensColetados()
    {
        int removidos = 0;
        
        // Estrelas
        foreach (var obj in FindObjectsOfType<StarPickup>())
            if (!string.IsNullOrEmpty(obj.idItem) && itensColetados.Contains(obj.idItem))
            { Destroy(obj.gameObject); removidos++; Debug.Log($"Estrela removida: {obj.idItem}"); }
        
        // Ervas
        foreach (var obj in FindObjectsOfType<ErvaCuraPickup>())
            if (!string.IsNullOrEmpty(obj.idItem) && itensColetados.Contains(obj.idItem))
            { Destroy(obj.gameObject); removidos++; Debug.Log($"Erva removida: {obj.idItem}"); }
        
        // Flores
        foreach (var obj in FindObjectsOfType<FlorPickup>())
            if (!string.IsNullOrEmpty(obj.idItem) && itensColetados.Contains(obj.idItem))
            { Destroy(obj.gameObject); removidos++; Debug.Log($"Flor removida: {obj.idItem}"); }
        
        // Pedras Azuis
        foreach (var obj in FindObjectsOfType<PedraAzulPickup>())
            if (!string.IsNullOrEmpty(obj.idItem) && itensColetados.Contains(obj.idItem))
            { Destroy(obj.gameObject); removidos++; Debug.Log($"PedraAzul removida: {obj.idItem}"); }
        
        // Moedas
        foreach (var obj in FindObjectsOfType<CoinPickup>())
            if (!string.IsNullOrEmpty(obj.idItem) && itensColetados.Contains(obj.idItem))
            { Destroy(obj.gameObject); removidos++; Debug.Log($"Moeda removida: {obj.idItem}"); }
        
        // Chaves
        foreach (var obj in FindObjectsOfType<KeyPickup>())
            if (!string.IsNullOrEmpty(obj.idItem) && itensColetados.Contains(obj.idItem))
            { Destroy(obj.gameObject); removidos++; Debug.Log($"Chave removida: {obj.idItem}"); }
        
        // Tochas
        foreach (var obj in FindObjectsOfType<TorchPickup>())
            if (!string.IsNullOrEmpty(obj.idItem) && itensColetados.Contains(obj.idItem))
            { Destroy(obj.gameObject); removidos++; Debug.Log($"Tocha removida: {obj.idItem}"); }
      
        Debug.Log($"Total de itens removidos da cena: {removidos}");
    }

    void RemoverInimigosMortos()
    {
        int removidos = 0;
        foreach (var aranha in FindObjectsOfType<Aranha>())
            if (!string.IsNullOrEmpty(aranha.idInimigo) && inimigosMortos.Contains(aranha.idInimigo))
            { Destroy(aranha.gameObject); removidos++; Debug.Log($"Aranha removida: {aranha.idInimigo}"); }
        
        foreach (var bat in FindObjectsOfType<BatEnemy>())
            if (!string.IsNullOrEmpty(bat.idInimigo) && inimigosMortos.Contains(bat.idInimigo))
            { Destroy(bat.gameObject); removidos++; Debug.Log($"Morcego removido: {bat.idInimigo}"); }
        
        Debug.Log($"Total de inimigos removidos: {removidos}");
    }

    public void ColetarItem(string itemId)
    {
        if (!itensColetados.Contains(itemId))
        {
            itensColetados.Add(itemId);
            Debug.Log($"Item REGISTRADO: {itemId}");
        }
    }

    public void DerrotarInimigo(string inimigoId)
    {
        if (!inimigosMortos.Contains(inimigoId))
        {
            inimigosMortos.Add(inimigoId);
            Debug.Log($"Inimigo REGISTRADO: {inimigoId}");
        }
    }
}