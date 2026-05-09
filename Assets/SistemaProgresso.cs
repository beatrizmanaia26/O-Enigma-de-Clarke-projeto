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
        StartCoroutine(RestaurarAposDelay());
    }
    
    IEnumerator RestaurarAposDelay()
    {
        yield return new WaitForSeconds(0.2f);
        
        string cenaAtual = SceneManager.GetActiveScene().name;
        
        if (GameStateManager.Instance != null && 
            GameStateManager.Instance.ExisteEstadoSalvo(cenaAtual))
        {
            RestaurarEstado();
        }
        else
        {
            Debug.Log($"Nenhum estado salvo para {cenaAtual}");
        }
    }
    
    public void RestaurarEstado()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        var estado = GameStateManager.Instance.RecuperarEstadoFase(cenaAtual);
        
        if (estado != null)
        {
            Debug.Log($"=== RESTAURANDO {cenaAtual} ===");
            
            // 1. Restaurar posição e vida do jogador
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
            
            // 2. Restaurar inventário
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.coins = estado.moedas;
                InventoryManager.Instance.stars = estado.estrelas;
                InventoryManager.Instance.blueStones = estado.pedrasAzuis;
                InventoryManager.Instance.flor = estado.flores;
                InventoryManager.Instance.crowns = estado.coroas;
                InventoryManager.Instance.ervaCura = estado.ervaCura;
                InventoryManager.Instance.pocao = estado.pocoes;
                InventoryManager.Instance.keys = estado.chaves;
                InventoryManager.Instance.torches = estado.tochas;
                InventoryManager.Instance.towerCrowns = estado.towerCrowns;
                InventoryManager.Instance.hasSword = estado.hasSword;
                Debug.Log($"Inventário restaurado: Moedas={estado.moedas}, Ervas={estado.ervaCura}");
            }
            
            // 3. Restaurar listas
            itensColetados = new List<string>(estado.itensColetados);
            inimigosMortos = new List<string>(estado.inimigosMortos);
            
            Debug.Log($"Itens coletados: {itensColetados.Count}, Inimigos mortos: {inimigosMortos.Count}");
            
            // Mostrar quais itens foram coletados
            foreach (string item in itensColetados)
            {
                Debug.Log($"  - Item coletado: {item}");
            }
            foreach (string inimigo in inimigosMortos)
            {
                Debug.Log($"  - Inimigo morto: {inimigo}");
            }
            
            // 4. Remover itens já coletados
            RemoverItensColetados();
            
            // 5. Remover inimigos já mortos
            RemoverInimigosMortos();
        }
    }
    
    void RemoverItensColetados()
    {
        int removidos = 0;
        
        // Remover Estrelas
        StarPickup[] estrelas = FindObjectsOfType<StarPickup>();
        foreach (var estrela in estrelas)
        {
            if (!string.IsNullOrEmpty(estrela.idItem) && itensColetados.Contains(estrela.idItem))
            {
                Destroy(estrela.gameObject);
                removidos++;
                Debug.Log($"Estrela removida: {estrela.idItem}");
            }
        }
        
        // Remover Ervas
        ErvaCuraPickup[] ervas = FindObjectsOfType<ErvaCuraPickup>();
        foreach (var erva in ervas)
        {
            if (!string.IsNullOrEmpty(erva.idItem) && itensColetados.Contains(erva.idItem))
            {
                Destroy(erva.gameObject);
                removidos++;
                Debug.Log($"Erva removida: {erva.idItem}");
            }
        }
        
        // Remover Flores
        FlorPickup[] flores = FindObjectsOfType<FlorPickup>();
        foreach (var flor in flores)
        {
            if (!string.IsNullOrEmpty(flor.idItem) && itensColetados.Contains(flor.idItem))
            {
                Destroy(flor.gameObject);
                removidos++;
                Debug.Log($"Flor removida: {flor.idItem}");
            }
        }
        
        // Remover Pedras Azuis
        PedraAzulPickup[] pedras = FindObjectsOfType<PedraAzulPickup>();
        foreach (var pedra in pedras)
        {
            if (!string.IsNullOrEmpty(pedra.idItem) && itensColetados.Contains(pedra.idItem))
            {
                Destroy(pedra.gameObject);
                removidos++;
                Debug.Log($"Pedra Azul removida: {pedra.idItem}");
            }
        }
        
        Debug.Log($"Total de itens removidos da cena: {removidos}");
    }
    
    void RemoverInimigosMortos()
    {
        int removidos = 0;
        
        // Remover Aranhas
        Aranha[] aranhas = FindObjectsOfType<Aranha>();
        foreach (var aranha in aranhas)
        {
            if (!string.IsNullOrEmpty(aranha.idInimigo) && inimigosMortos.Contains(aranha.idInimigo))
            {
                Destroy(aranha.gameObject);
                removidos++;
                Debug.Log($"Aranha removida: {aranha.idInimigo}");
            }
        }
        
        // Remover Morcegos
        BatEnemy[] bats = FindObjectsOfType<BatEnemy>();
        foreach (var bat in bats)
        {
            if (!string.IsNullOrEmpty(bat.idInimigo) && inimigosMortos.Contains(bat.idInimigo))
            {
                Destroy(bat.gameObject);
                removidos++;
                Debug.Log($"Morcego removido: {bat.idInimigo}");
            }
        }
        
        Debug.Log($"Total de inimigos removidos da cena: {removidos}");
    }
    
    public void ColetarItem(string itemId)
    {
        if (!itensColetados.Contains(itemId))
        {
            itensColetados.Add(itemId);
            Debug.Log($"Item registrado: {itemId}");
        }
    }
    
    public void DerrotarInimigo(string inimigoId)
    {
        if (!inimigosMortos.Contains(inimigoId))
        {
            inimigosMortos.Add(inimigoId);
            Debug.Log($"Inimigo registrado: {inimigoId}");
        }
    }
}