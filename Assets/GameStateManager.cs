// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class GameStateManager : MonoBehaviour
// {
//     public static GameStateManager Instance { get; private set; }
    
//     [System.Serializable]
//     public class FaseState
//     {
//         public string nomeFase;
//         public Vector3 posicaoJogador;
//         public float vidaJogador;
//         public int moedas;
//         public int estrelas;
//         public int pedrasAzuis;
//         public int flores;
//         public int coroas;
//         public int ervaCura;
//         public int pocoes;
//         public int chaves;
//         public int tochas;
//         public int towerCrowns;
//         public bool hasSword;
//         public List<string> itensColetados = new List<string>();
//         public List<string> inimigosMortos = new List<string>();
//     }
    
//     private FaseState estadoAtual;
//     private bool inicializado = false;
    
//     void Awake()
//     {
//         // LOG PARA DEBUG
//         Debug.Log($"GameStateManager.Awake() na cena: {SceneManager.GetActiveScene().name}");
        
//         // Singleton pattern CORRETO
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//             inicializado = true;
//             Debug.Log($"GameStateManager CRIADO e persistente na cena: {SceneManager.GetActiveScene().name}");
//         }
//         else
//         {
//             Debug.Log($"GameStateManager já existe, destruindo este. Cena atual: {SceneManager.GetActiveScene().name}");
//             Destroy(gameObject);
//         }
//     }
    
//     void OnEnable()
//     {
//         // Se inscrever no evento de carregamento de cena
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }
    
//     void OnDisable()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }
    
//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         Debug.Log($"GameStateManager: Cena carregada: {scene.name}. Estado atual existe: {estadoAtual != null}");
//     }
    
//     public void SalvarEstadoFase(string nomeFase, GameObject jogador, SistemaProgresso progresso)
//     {
//         Debug.Log($"SalvarEstadoFase chamado para: {nomeFase}");
        
//         estadoAtual = new FaseState();
//         estadoAtual.nomeFase = nomeFase;
        
//         if (jogador != null)
//         {
//             estadoAtual.posicaoJogador = jogador.transform.position;
//             Debug.Log($"Posição salva: {estadoAtual.posicaoJogador}");
            
//             var playerScript = jogador.GetComponent<Player>();
//             if (playerScript != null) 
//             {
//                 estadoAtual.vidaJogador = playerScript.VidasRestantes;
//                 Debug.Log($"Vida salva: {estadoAtual.vidaJogador}");
//             }
//         }
//         else
//         {
//             Debug.LogWarning("Jogador é NULL ao salvar!");
//         }
        
//         if (InventoryManager.Instance != null)
//         {
//             estadoAtual.moedas = InventoryManager.Instance.coins;
//             estadoAtual.estrelas = InventoryManager.Instance.stars;
//             estadoAtual.pedrasAzuis = InventoryManager.Instance.blueStones;
//             estadoAtual.flores = InventoryManager.Instance.flor;
//             estadoAtual.coroas = InventoryManager.Instance.crowns;
//             estadoAtual.ervaCura = InventoryManager.Instance.ervaCura;
//             estadoAtual.pocoes = InventoryManager.Instance.pocao;
//             estadoAtual.chaves = InventoryManager.Instance.keys;
//             estadoAtual.tochas = InventoryManager.Instance.torches;
//             estadoAtual.towerCrowns = InventoryManager.Instance.towerCrowns;
//             estadoAtual.hasSword = InventoryManager.Instance.hasSword;
//             Debug.Log($"Inventário salvo: Moedas={estadoAtual.moedas}, Ervas={estadoAtual.ervaCura}");
//         }
        
//         if (progresso != null)
//         {
//             estadoAtual.itensColetados = new List<string>(progresso.itensColetados);
//             estadoAtual.inimigosMortos = new List<string>(progresso.inimigosMortos);
//         }
        
//         PlayerPrefs.SetString("UltimaFase", nomeFase);
//         PlayerPrefs.Save();
        
//         Debug.Log($"=== FASE SALVA COM SUCESSO! ===\nFase: {nomeFase}\nPosição: {estadoAtual.posicaoJogador}\n=================================");
//     }
    
//     public FaseState RecuperarEstadoFase(string nomeFase)
//     {
//         if (estadoAtual != null && estadoAtual.nomeFase == nomeFase)
//         {
//             Debug.Log($"Estado ENCONTRADO para fase: {nomeFase}");
//             return estadoAtual;
//         }
        
//         Debug.Log($"Estado NÃO encontrado para fase: {nomeFase}. EstadoAtual é null? {estadoAtual == null}");
//         return null;
//     }
    
//     public bool ExisteEstadoSalvo(string nomeFase)
//     {
//         bool existe = estadoAtual != null && estadoAtual.nomeFase == nomeFase;
//         Debug.Log($"ExisteEstadoSalvo para {nomeFase}: {existe}");
//         return existe;
//     }
    
//     public void LimparEstadoSalvo()
//     {
//         estadoAtual = null;
//         PlayerPrefs.DeleteKey("UltimaFase");
//         Debug.Log("Estado salvo limpo!");
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [System.Serializable]
    public class FaseState
    {
        public string nomeFase;
        public Vector3 posicaoJogador;
        public float vidaJogador = 5;

        public int moedas;
        public int estrelas;
        public int pedrasAzuis;
        public int flores;
        public int coroas;
        public int ervaCura;
        public int pocoes;
        public int chaves;
        public int tochas;
        public int towerCrowns;
        public bool hasSword;

        public List<string> itensColetados = new List<string>();
        public List<string> inimigosMortos = new List<string>();
    }

    private FaseState estadoAtual;

    void Awake()
    {
        Debug.Log($"GameStateManager.Awake() na cena: {SceneManager.GetActiveScene().name}");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"GameStateManager CRIADO e persistente na cena: {SceneManager.GetActiveScene().name}");
        }
        else
        {
            Debug.Log($"GameStateManager já existe, destruindo este. Cena atual: {SceneManager.GetActiveScene().name}");
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"GameStateManager: Cena carregada: {scene.name}. Estado atual existe: {estadoAtual != null}");
    }

    public void SalvarEstadoFase(string nomeFase, GameObject jogador, SistemaProgresso progresso)
    {
        Debug.Log($"SalvarEstadoFase chamado para: {nomeFase}");

        estadoAtual = new FaseState();
        estadoAtual.nomeFase = nomeFase;

        if (jogador != null)
        {
            estadoAtual.posicaoJogador = jogador.transform.position;
            estadoAtual.vidaJogador = 5;

            Debug.Log($"Posição salva: {estadoAtual.posicaoJogador}");

            // Player usado na fase 3
            var playerScript = jogador.GetComponent<Player>();
            if (playerScript != null)
            {
                estadoAtual.vidaJogador = playerScript.VidasRestantes;
                estadoAtual.posicaoJogador = playerScript.GetPosition();
                Debug.Log($"Vida salva pelo Player: {estadoAtual.vidaJogador}");
            }

            // Player usado na fase 1/2
            var playerFase2 = jogador.GetComponent<PlayerFase2>();
            if (playerFase2 != null)
            {
                estadoAtual.vidaJogador = playerFase2.VidasRestantes;
                estadoAtual.posicaoJogador = playerFase2.GetPosition();
                Debug.Log($"Vida salva pelo PlayerFase2: {estadoAtual.vidaJogador}");
            }

            // Segurança para não salvar vida zerada por erro
            if (estadoAtual.vidaJogador <= 0)
            {
                Debug.LogWarning("Vida estava 0 ao salvar. Ajustando para 5 para evitar sumir todos os corações.");
                estadoAtual.vidaJogador = 5;
            }
        }
        else
        {
            Debug.LogWarning("Jogador é NULL ao salvar!");
            estadoAtual.vidaJogador = 5;
        }

        if (InventoryManager.Instance != null)
        {
            estadoAtual.moedas = InventoryManager.Instance.coins;
            estadoAtual.estrelas = InventoryManager.Instance.stars;
            estadoAtual.pedrasAzuis = InventoryManager.Instance.blueStones;
            estadoAtual.flores = InventoryManager.Instance.flor;
            estadoAtual.coroas = InventoryManager.Instance.crowns;
            estadoAtual.ervaCura = InventoryManager.Instance.ervaCura;
            estadoAtual.pocoes = InventoryManager.Instance.pocao;
            estadoAtual.chaves = InventoryManager.Instance.keys;
            estadoAtual.tochas = InventoryManager.Instance.torches;
            estadoAtual.towerCrowns = InventoryManager.Instance.towerCrowns;
            estadoAtual.hasSword = InventoryManager.Instance.hasSword;

            Debug.Log($"Inventário salvo: Moedas={estadoAtual.moedas}, Ervas={estadoAtual.ervaCura}, Espada={estadoAtual.hasSword}");
        }

        if (progresso != null)
        {
            estadoAtual.itensColetados = new List<string>(progresso.itensColetados);
            estadoAtual.inimigosMortos = new List<string>(progresso.inimigosMortos);

            Debug.Log($"Itens salvos: {estadoAtual.itensColetados.Count}");
            Debug.Log($"Inimigos salvos: {estadoAtual.inimigosMortos.Count}");
        }

        PlayerPrefs.SetString("UltimaFase", nomeFase);
        PlayerPrefs.Save();

        Debug.Log(
            $"=== FASE SALVA COM SUCESSO! ===\n" +
            $"Fase: {nomeFase}\n" +
            $"Posição: {estadoAtual.posicaoJogador}\n" +
            $"Vida: {estadoAtual.vidaJogador}\n" +
            $"================================="
        );
    }

    public FaseState RecuperarEstadoFase(string nomeFase)
    {
        if (estadoAtual != null && estadoAtual.nomeFase == nomeFase)
        {
            Debug.Log($"Estado ENCONTRADO para fase: {nomeFase}");
            return estadoAtual;
        }

        Debug.Log($"Estado NÃO encontrado para fase: {nomeFase}. EstadoAtual é null? {estadoAtual == null}");
        return null;
    }

    public bool ExisteEstadoSalvo(string nomeFase)
    {
        bool existe = estadoAtual != null && estadoAtual.nomeFase == nomeFase;
        Debug.Log($"ExisteEstadoSalvo para {nomeFase}: {existe}");
        return existe;
    }

    public void LimparEstadoSalvo()
    {
        estadoAtual = null;
        PlayerPrefs.DeleteKey("UltimaFase");
        Debug.Log("Estado salvo limpo!");
    }
}