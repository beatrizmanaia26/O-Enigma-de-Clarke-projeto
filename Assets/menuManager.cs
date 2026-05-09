using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuNavegacao : MonoBehaviour
{
    private GameObject jogador;
    private SistemaProgresso sistemaProgresso;
    
    void Start()
    {
        // Não tentar encontrar jogador em todas as cenas
        // Só encontrar se for uma cena de jogo (não menu)
        string cenaAtual = SceneManager.GetActiveScene().name;
        if (cenaAtual.Contains("fase") || cenaAtual.Contains("Jogo"))
        {
            EncontrarJogadorEProgresso();
        }
    }
    
    void EncontrarJogadorEProgresso()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
        {
            sistemaProgresso = jogador.GetComponent<SistemaProgresso>();
            Debug.Log("Jogador e SistemaProgresso encontrados na fase!");
        }
        else
        {
            Debug.LogWarning("Jogador NÃO encontrado! Verifique a Tag 'Player'");
        }
    }
    
    // Botão de Pause chamado DENTRO DA FASE
    public void VoltarTelaPause()
    {
        Debug.Log("=== BOTÃO PAUSE CLICADO NA FASE ===");
        
        // Primeiro: Verificar se GameStateManager existe
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("GameStateManager não existe! Criando um agora...");
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameStateManager>();
        }
        
        // Segundo: Encontrar o jogador na fase ATUAL (antes de sair)
        EncontrarJogadorEProgresso();
        
        if (jogador == null)
        {
            Debug.LogError("Jogador não encontrado na fase! Não posso salvar!");
            SceneManager.LoadScene("telaPause");
            return;
        }
        
        // Terceiro: Salvar o estado
        string faseAtual = SceneManager.GetActiveScene().name;
        Debug.Log($"Salvando fase: {faseAtual}");
        Debug.Log($"Posição do jogador: {jogador.transform.position}");
        
        GameStateManager.Instance.SalvarEstadoFase(faseAtual, jogador, sistemaProgresso);
        
        // Quarto: Salvar no PlayerPrefs qual fase era
        PlayerPrefs.SetString("UltimaFase", faseAtual);
        PlayerPrefs.Save();
        
        // Quinto: Ir para tela de pause
        Debug.Log("Indo para tela de pause...");
        SceneManager.LoadScene("telaPause");
    }
    
    // Botão Voltar chamado DENTRO DA TELA PAUSE
    public void VoltarFase()
    {
        Debug.Log("=== BOTÃO VOLTAR CLICADO NA TELA PAUSE ===");
        
        // Verificar se GameStateManager existe
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("GameStateManager não existe!");
            SceneManager.LoadScene("fase1Jogo");
            return;
        }
        
        // Recuperar qual era a última fase
        string ultimaFase = PlayerPrefs.GetString("UltimaFase", "");
        Debug.Log($"Última fase salva: '{ultimaFase}'");
        
        if (string.IsNullOrEmpty(ultimaFase))
        {
            Debug.LogWarning("Nenhuma fase salva! Voltando para fase 1");
            SceneManager.LoadScene("fase1Jogo");
            return;
        }
        
        // Carregar a fase (o SistemaProgresso vai restaurar automaticamente no Start)
        Debug.Log($"Carregando fase: {ultimaFase}");
        SceneManager.LoadScene(ultimaFase);
    }
    
    // ========== MÉTODOS PARA COMECAR FASES ==========
    public void JogarFase1() 
    { 
        PlayerPrefs.SetString("UltimaFase", "fase1Jogo");
        PlayerPrefs.Save();
        SceneManager.LoadScene("fase1Jogo");
    }
    
    public void JogarFase2() 
    { 
        PlayerPrefs.SetString("UltimaFase", "fase2Jogo");
        PlayerPrefs.Save();
        SceneManager.LoadScene("fase2Jogo");
    }
    
    public void JogarFase3() 
    { 
        PlayerPrefs.SetString("UltimaFase", "fase3Jogo");
        PlayerPrefs.Save();
        SceneManager.LoadScene("fase3Jogo");
    }
    
    public void JogarFase4() 
    { 
        PlayerPrefs.SetString("UltimaFase", "fase4Jogo");
        PlayerPrefs.Save();
        SceneManager.LoadScene("fase4Jogo");
    }
    
    public void IrParaContinuar()
    {
        string ultimaFase = PlayerPrefs.GetString("UltimaFase", "fase1Jogo");
        SceneManager.LoadScene(ultimaFase);
    }
    
    // ========== SEUS OUTROS MÉTODOS ==========
    public void IrParaNovoJogo() 
    { 
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.LimparEstadoSalvo();
        
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.coins = 0;
            InventoryManager.Instance.stars = 0;
            InventoryManager.Instance.blueStones = 0;
            InventoryManager.Instance.flor = 0;
            InventoryManager.Instance.crowns = 0;
            InventoryManager.Instance.ervaCura = 0;
            InventoryManager.Instance.pocao = 0;
            InventoryManager.Instance.keys = 0;
            InventoryManager.Instance.torches = 0;
            InventoryManager.Instance.towerCrowns = 0;
            InventoryManager.Instance.hasSword = false;
        }
        
        SceneManager.LoadScene("novoJogo"); 
    }
    
    public void IrParaOpcoes() { SceneManager.LoadScene("opcoes"); }
    public void IrParaHistoriaPersonagens() { SceneManager.LoadScene("historiaPersonagens"); }
    public void Voltar() { SceneManager.LoadScene("telaInicial"); }
    public void IrParaInventario() { SceneManager.LoadScene("inventario"); }
    public void IrParaMapa() { SceneManager.LoadScene("mapaJogo"); }
    public void irFase1() { SceneManager.LoadScene("fase1TelaInicial"); }
    public void irFase2() { SceneManager.LoadScene("fase2TelaInicial"); }
    public void irFase3() { SceneManager.LoadScene("fase3TelaInicial"); }
    public void irFase4() { SceneManager.LoadScene("fase4TelaInicial"); }
    
    public void CarregarHistoriapt2() { SceneManager.LoadScene("novoJogoHistoria2"); }
    public void CarregarHistoriapt3() { SceneManager.LoadScene("novoJogoHistoria3"); }
    public void CarregarHistoriapt4() { SceneManager.LoadScene("novoJogoHistoria4"); }
    public void CarregarHistoriapt5() { SceneManager.LoadScene("novoJogoHistoria5"); }
    public void CarregarHistoriaLobo() { SceneManager.LoadScene("historiaPersonagensLobo"); }
    public void CarregarHistoriaMoradorFerido() { SceneManager.LoadScene("historiaPersonagensMoradorFerido"); }
    public void CarregarHistoriaGuerreiroFerido() { SceneManager.LoadScene("historiaPersonagensGuerreiroFerido"); }
    public void CarregarHistoriaGnomo() { SceneManager.LoadScene("historiaPersonagensGnomo"); }
    public void CarregarHistoriaMago() { SceneManager.LoadScene("historiaPersonagensMago"); }
    public void CarregarHistoriaAranhaBau() { SceneManager.LoadScene("historiaPersonagensAranhaBau"); }
    public void CarregarHistoriaBruxa() { SceneManager.LoadScene("historiaPersonagensBruxa"); }
}